using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoStore
{
    public static class TodoApi
    {
        [FunctionName("AddTodo")]
        public static async Task<IActionResult> AddTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route ="todo")]
            HttpRequest request,
            [Table("todo",Connection="AzureWebJobsStorage")]
            IAsyncCollector<TodoEntity> table,
            ILogger logger
            )
        {
            logger.LogInformation("Received request for new to do..");
            string requestString = await request.ReadAsStringAsync();
            logger.LogInformation($"Request {requestString}");
            var objectToCreate = JsonConvert.DeserializeObject<TodoRequestModel>(requestString);
            await table.AddAsync(objectToCreate.MapTodoToEntity());
            logger.LogInformation($"Create {objectToCreate}");
            return new OkObjectResult(objectToCreate);
        }

        [FunctionName("GetAllTodo")]
        public static async Task<IActionResult> GetAllTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route ="todo")]
            HttpRequest request,
            [Table("todo",Connection ="AzureWebJobsStorage")]
            CloudTable todoTable,
            ILogger logger
            )
        {
            var query = new TableQuery<TodoEntity>();
            var segment = await todoTable.ExecuteQuerySegmentedAsync(query, null);
            return new OkObjectResult(segment.Select(Extentions.MapEntityToTodo));
        }

        [FunctionName("RunToDoChecks")]
        public static void RunTimer(
            [TimerTrigger("0 */5 * * * *")]
            TimerInfo timer, 
            ILogger logger)
        {
            logger.LogInformation($"Timer ran {DateTime.Now}.");
        }
    }
}
