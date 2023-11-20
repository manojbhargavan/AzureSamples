using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.Azure.Cosmos;
using cosmosDb;
using System.Threading.Tasks;
using Humanizer;

namespace CosmosDb
{
    class Program
    {
        static void Main(string[] args)
        {
            //Read Configuration
            var configuration = new ConfigurationBuilder()
                                    .AddJsonFile("appsettings.json")
                                    .Build();

            //Read Data
            string volcanosString = File.ReadAllText("data.json");
            List<Volcano> volcanos = JsonConvert.DeserializeObject<List<Volcano>>(volcanosString);

            #region Push 20 Objects / Upsert         
            using (CosmosClient cosmosClient = new CosmosClient(configuration["CosmosHost"], configuration["CosmosKey"]))
            {
                var objectToPush = volcanos.Take(20);
                System.Console.WriteLine(objectToPush);

                var containerHandle = cosmosClient.GetContainer(configuration["DatabaseId"], configuration["ContainerId"]);
                objectToPush.ToList().ForEach(v =>
                {
                    var result = containerHandle.UpsertItemAsync(v).Result;
                    System.Console.WriteLine(JsonConvert.SerializeObject(result));
                    System.Console.WriteLine();
                });
                #endregion

                #region Read Single Item 
                var readdata = containerHandle.ReadItemAsync<Volcano>(objectToPush.First().id, new PartitionKey(objectToPush.First().type)).Result;
                System.Console.WriteLine();
                System.Console.WriteLine("Read Data");
                System.Console.WriteLine(readdata.Resource);
                #endregion

                #region Read based on a query
                QueryDefinition queryDefinition = new QueryDefinition("select * from c");

                using (var iterator = containerHandle.GetItemQueryIterator<Volcano>(queryDefinition))
                {
                    int count = 0;
                    while (iterator.HasMoreResults)
                    {
                        foreach (var item in iterator.ReadNextAsync().Result)
                        {
                            System.Console.WriteLine();
                            System.Console.WriteLine("--------------------------");
                            System.Console.WriteLine($"Item {count++}");
                            System.Console.WriteLine("--------------------------");
                            System.Console.WriteLine(item);
                            System.Console.WriteLine("--------------------------");
                        }
                    }
                }
                #endregion

            }

            #region Push objects in Bulk
            using (CosmosClient cosmosClient = new CosmosClient(configuration["CosmosHost"], configuration["CosmosKey"],
                                                                    //Allow Bulk
                                                                    new CosmosClientOptions() { AllowBulkExecution = true }))
            {
                var startTime = DateTime.UtcNow;
                System.Console.WriteLine("Started //el Upsert");

                var containerHandle = cosmosClient.GetContainer(configuration["DatabaseId"], configuration["ContainerId"]);
                List<Task> concInserts = new List<Task>();

                foreach (var volcano in volcanos)
                {
                    concInserts.Add(containerHandle.UpsertItemAsync(volcano, new PartitionKey(volcano.type)));

                }

                Task.WhenAll(concInserts).Wait();
                var endTime = DateTime.UtcNow;
                System.Console.WriteLine("Done //el Upsert");
                System.Console.WriteLine((endTime - startTime).Humanize());
            }
            #endregion

        }
    }
}
