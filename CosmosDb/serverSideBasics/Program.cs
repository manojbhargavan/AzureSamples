using System;
using System.Collections.Generic;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using serverSideBasics.SP;
using serverSideBasics.Transaction;
using serverSideBasics.Trigger;

namespace ServerSideBasics
{
    class Program
    {
        static void Main(string[] args)
        {
            //Read Configuration
            var configuration = new ConfigurationBuilder()
                                    .AddJsonFile("appsettings.json")
                                    .Build();

            //Stored Procedure Demo
            ShowMessage("Stored Procedure Demo");
            SpDemo.StoredProcedureToInsertDocuments(configuration);

            //Pre Trigger
            ShowMessage("Trigger Demo");
            PreTriggerDemo.TriggerWhileInsertDocuments(configuration);

            //Transaction using the SDK
            ShowMessage("Transaction Demo");
            TransactionsDemo.TransactionUsingSdk(configuration);
        }

        private static void ShowMessage(string message)
        {
            System.Console.WriteLine("Press Enter to continue...");
            System.Console.ReadLine();
            System.Console.WriteLine(message);
        }
    }
}
