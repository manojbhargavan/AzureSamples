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

namespace EmployeeRepo
{
    public static class Employee
    {
        [FunctionName("employeeGetAll")]
        public static IActionResult EmployeeGetAll(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "employee")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Getting Employee Data from Repo");

            IEmployeeRepository empData = new EmployeeRepository();
            var data = empData.GetEmployees();

            return new JsonResult(data);
        }

        [FunctionName("employeeGet")]
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
    }
}
