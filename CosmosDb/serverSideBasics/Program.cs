using System;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace serverSideBasics
{
    class Program
    {
        static void Main(string[] args)
        {
            //Read Configuration
            var configuration = new ConfigurationBuilder()
                                    .AddJsonFile("appsettings.json")
                                    .Build();

                                    using (DocumentClient cosmosDocumentClient = new DocumentClient(new Uri(configuration["CosmosHost"]), configuration["CosmosKey"]))
            {
                var emp1 = new employee()
                {
                    firstName = "Manoj",
                    lastName = "test",
                    department = "IT",
                    location = "Hyderabad",
                    id = Guid.NewGuid().ToString()
                };

                var emp2 = new employee()
                {
                    firstName = "Nandan",
                    lastName = "test",
                    department = "IT",
                    location = "Hyderabad",
                    id = Guid.NewGuid().ToString()
                };

                var emp3 = new employee()
                {
                    firstName = "ErrorKing",
                    lastName = "test",
                    department = "IT",
                    location = "HyderabadKing",
                    id = Guid.NewGuid().ToString()
                };

                var storedProcResult = cosmosDocumentClient.ExecuteStoredProcedureAsync<object>
                                        (UriFactory.CreateStoredProcedureUri(configuration["DatabaseId"], "employee", "createEmployees"),
                                        new Microsoft.Azure.Documents.Client.RequestOptions
                                        {
                                            PartitionKey = new Microsoft.Azure.Documents.PartitionKey("Hyderabad")
                                        }, JsonConvert.SerializeObject(new object[] { emp1, emp2, emp3 })).Result;
                System.Console.WriteLine(storedProcResult.Response);
            }
        }
    }
}
