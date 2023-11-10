using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageWrapper.Helper
{
    public class TableHelper
    {
        private readonly CloudStorageAccount cloudStorageAccount;

        public TableHelper()
        {
            cloudStorageAccount = CloudStorageAccount.Parse(Constants.StorageAccountConnectionString);
        }

        public CloudTable CreateTable(string tableName)
        {
            CloudTableClient tableClient = cloudStorageAccount.CreateCloudTableClient();
            CloudTable tableReference = tableClient.GetTableReference(tableName);

            if (tableReference.CreateIfNotExists())
                Console.WriteLine("Table created");
            else
                Console.WriteLine("Table exits");

            return tableReference;
        }

        public T InsertEntity<T>(CloudTable table, T entity) where T : TableEntity
        {
            TableOperation tableOperation = TableOperation.InsertOrMerge(entity);
            TableResult tableResult = table.Execute(tableOperation);

            T insertedT = tableResult.Result as T;

            if (tableResult.RequestCharge.HasValue)
            {
                Console.WriteLine("Request Charge of InsertOrMerge Operation: " + tableResult.RequestCharge);
            }

            return insertedT;
        }
    }
}
