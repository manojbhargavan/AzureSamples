using System;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using serverSideBasics.Data;
using ServerSideBasics;

namespace serverSideBasics.SP
{
    internal static class SpDemo
    {
        internal static void StoredProcedureToInsertDocuments(IConfigurationRoot configuration)
        {
            using (DocumentClient cosmosDocumentClient = new DocumentClient(new Uri(configuration["CosmosHost"]), configuration["CosmosKey"]))
            {
                employee emp1, emp2, emp3;
                EmployeeMockData.MockEmployeeDataOutError(out emp1, out emp2, out emp3);

                var storedProcResult = cosmosDocumentClient.ExecuteStoredProcedureAsync<object>
                                        (UriFactory.CreateStoredProcedureUri(configuration["DatabaseId"], "employee", "createEmployees"),
                                        new RequestOptions
                                        {
                                            PartitionKey = new Microsoft.Azure.Documents.PartitionKey("Hyderabad")
                                        }, JsonConvert.SerializeObject(new object[] { emp1, emp2, emp3 })).Result;
                Console.WriteLine(storedProcResult.Response);
            }
        }


    }
}