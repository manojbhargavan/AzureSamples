using System;
using System.Collections.Generic;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using serverSideBasics.Data;
using ServerSideBasics;

namespace serverSideBasics.Transaction
{
    internal class TransactionsDemo
    {
        internal static void TransactionUsingSdk(IConfigurationRoot configuration)
        {
            employee emp1, emp2, emp3;
            EmployeeMockData.MockEmployeeDataOutError(out emp1, out emp2, out emp3);

            employee emp4, emp5, emp6;
            EmployeeMockData.MockEmployeeDataOutNoError(out emp4, out emp5, out emp6);

            using (CosmosClient cosmosClient = new CosmosClient(configuration["CosmosHost"], configuration["CosmosKey"]))
            {
                var containerHandle = cosmosClient.GetContainer(configuration["DatabaseId"], "employee");

                //Error Batch
                var resultError = containerHandle.CreateTransactionalBatch(new PartitionKey("Hyderabad"))
                    .CreateItem(emp1)
                    .CreateItem(emp2)
                    .CreateItem(emp3)
                    .ExecuteAsync()
                    .Result;

                RenderResult(resultError, 3);

                //Error Batch Without Error record
                var resultTwoItems = containerHandle.CreateTransactionalBatch(new PartitionKey("Hyderabad"))
                    .CreateItem(emp1)
                    .CreateItem(emp2)
                    .ExecuteAsync()
                    .Result;

                RenderResult(resultTwoItems, 2);

                //Ok Batch
                var resultOk = containerHandle.CreateTransactionalBatch(new PartitionKey("Hyderabad"))
                    .UpsertItem(emp1)
                    .UpsertItem(emp2)
                    .CreateItem(emp4)
                    .CreateItem(emp5)
                    .CreateItem(emp6)
                    .ExecuteAsync()
                    .Result;

                RenderResult(resultOk, 5);
            }
        }

        private static void RenderResult(TransactionalBatchResponse resultError, int resultCount)
        {
            Console.WriteLine();
            Console.WriteLine("Result View------------------------------------------");
            Console.WriteLine($"Overall Status: {resultError.IsSuccessStatusCode}");
            if (!resultError.IsSuccessStatusCode)
                Console.WriteLine($"Error Message: {resultError.ErrorMessage}");
            Console.WriteLine($"Status Code: {resultError.StatusCode}");
            Console.WriteLine($"Activity Id: {resultError.ActivityId}");


            for (int i = 0; i < resultCount; i++)
            {
                Console.WriteLine($"\tSub Task #{i + 1}------------------------------------------");
                var operationResult = resultError.GetOperationResultAtIndex<dynamic>(i);
                Console.WriteLine($"\t Status Code: {operationResult.StatusCode}");
                Console.WriteLine($"\t ETag: {operationResult.ETag}");
                Console.WriteLine($"\t Resource: {operationResult.Resource}");
                Console.WriteLine($"\t Status: {operationResult.IsSuccessStatusCode}");
                Console.WriteLine($"\tSub Task #{i + 1} End---------------------------------------");
                Console.WriteLine();
            }
            Console.WriteLine("Result View End---------------------------------------");
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
            Console.WriteLine();
        }
    }
}