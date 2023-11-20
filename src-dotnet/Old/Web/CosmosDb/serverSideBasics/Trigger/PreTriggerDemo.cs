using System;
using System.Collections.Generic;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using serverSideBasics.Data;
using ServerSideBasics;

namespace serverSideBasics.Trigger
{
    internal static class PreTriggerDemo
    {
        internal static void TriggerWhileInsertDocuments(IConfigurationRoot configuration)
        {
            List<employee> empList = EmployeeMockData.MockEmployeeDataList();

            using (CosmosClient cosmosClient = new CosmosClient(configuration["CosmosHost"], configuration["CosmosKey"]))
            {
                var objectToPush = empList;

                var containerHandle = cosmosClient.GetContainer(configuration["DatabaseId"], "employee");
                objectToPush.ForEach(v =>
                {
                    var result = containerHandle.UpsertItemAsync(v,
                        requestOptions: new ItemRequestOptions()
                        {
                            PreTriggers = new List<string>() { "setFullName" }
                        }).Result;
                    Console.WriteLine(JsonConvert.SerializeObject(result));
                    Console.WriteLine();
                });
            }
        }


    }
}