using System;
using System.Globalization;
using System.IO;
using CsvHelper;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System.Linq;
using Newtonsoft.Json;


namespace EmployeeDataProcessor
{
    public class EmployeeCsvProcessor
    {
        [FunctionName("EmployeeCsvProcessor")]
        public void Run([BlobTrigger("employeerawdata/{name}", Connection = "EmployeeStorage")] Stream myBlob, string name, ILogger log, Microsoft.Azure.WebJobs.Binder binder)
        {
            log.LogInformation($"File Received --> Name:{name}, Size: {myBlob.Length} Bytes");

            string outputContainer = "employees";

            using (var reader = new StreamReader(myBlob))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<Employee>()?.ToList();
                records.ForEach(e =>
                {
                    string data = JsonConvert.SerializeObject(e);
                    log.LogInformation(data);
                    string fileName = $"{outputContainer}/{e.EmployeeID}.json";

                    var attributes = new Attribute[]
                    {
                        new BlobAttribute(fileName),
                        new StorageAccountAttribute("EmployeeStorage")
                    };


                    using (var writer = binder.BindAsync<TextWriter>(attributes).Result)
                    {
                        writer.Write(data);
                    }
                });

            }
        }
    }
}
