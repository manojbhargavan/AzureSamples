using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using EmployeeRepo.Data;
using EmployeeRepo.Data.FileSystem;
using System.Net.Http;

namespace EmployeeRepo
{
    public static class Employee
    {
        [FunctionName("getAll")]
        public static IActionResult EmployeeGetAll(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "employee")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Getting Employee Data from Repo");

            IEmployeeRepository empData = new EmployeeRepository();
            var data = empData.GetEmployees();

            return new JsonResult(data);
        }

        [FunctionName("get")]
        public static IActionResult EmployeeGet(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "employee/{id}")] HttpRequest req,
            long id,
            ILogger log)
        {
            log.LogInformation("Getting Employee Data from Repo");

            IEmployeeRepository empData = new EmployeeRepository();
            var data = empData.GetEmployee(id);

            if (data == null)
                return new NotFoundObjectResult($"Not Found: {id}");

            return new JsonResult(data);
        }

        [FunctionName("delete")]
        public static IActionResult EmployeeDelete(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "employee/{id}")] HttpRequest req,
            long id,
            ILogger log)
        {
            log.LogInformation("Deleting Employee Data from Repo");

            IEmployeeRepository empData = new EmployeeRepository();
            var data = empData.GetEmployee(id);

            if (data == null)
                return new NotFoundObjectResult($"Not Found: {id}");

            var result = empData.DeleteEmployee(id);

            if (result)
                return new NoContentResult();
            else
                return new BadRequestResult();
        }

        [FunctionName("update")]
        public static async Task<IActionResult> EmployeeUpdate(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "employee")] HttpRequestMessage req,            
            ILogger log)
        {
            try
            {
                string reqBody = await req.Content.ReadAsStringAsync();
                Data.Employee employee = JsonConvert.DeserializeObject<Data.Employee>(reqBody);
                log.LogInformation("Updating Employee Data from Repo");

                IEmployeeRepository empData = new EmployeeRepository();
                if (employee != null)
                {
                    var result = empData.UpdateEmployee(employee);

                    if (result)
                        return new NoContentResult();
                    else
                        return new BadRequestResult();
                }
                else
                {
                    return new NotFoundObjectResult($"Not Found/Unable to update");
                }
            }
            catch(Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }
        }


    }
}
