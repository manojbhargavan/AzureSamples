using System;
using System.Globalization;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using CsvHelper;
using System.Linq;
using Newtonsoft.Json;

namespace InventoryProcessor
{
    public class InventoryJsonGen
    {
        [FunctionName("InventoryJsonGen")]
        public void Run([BlobTrigger("rawinventory/{name}", Connection = "InventoryStorage")] Stream myBlob,
                            string name, ILogger log, Binder binder)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
            using (var reader = new StreamReader(myBlob))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var inventoryData = csv.GetRecords<InventoryDto>().ToList();

                foreach (var curItem in inventoryData)
                {
                    var outputContainerName = "inventory";
                    var fileName = $"{outputContainerName}/{curItem.Id}.json";
                    var storageAccountAttribute = new StorageAccountAttribute("InventoryStorage");
                    var blobAttribute = new BlobAttribute(fileName);
                    Attribute[] fileAttributes = { storageAccountAttribute, blobAttribute };

                    using (var writer = binder.BindAsync<TextWriter>(fileAttributes).Result)
                    {
                        writer.Write(JsonConvert.SerializeObject(curItem.Map()));
                        log.LogInformation($"Blob uploaded {fileName}");
                    }

                }
            }
        }
    }
}
