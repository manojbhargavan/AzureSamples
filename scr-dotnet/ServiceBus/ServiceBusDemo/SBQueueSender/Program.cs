using Azure.Messaging.ServiceBus;
using LoremNET;
using Newtonsoft.Json;
using SBShared.Models;

namespace SBQueueSender
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // Connection Details - Get from a Vault
            string connectionString = "Endpoint=sb://snprac007.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=Dc3tXophUaaOpw03ZePeseT9FW+i8tsJA+ASbIEkAtA=";
            string queueName = "simplequeue";

            // SB Client
            var sbClient = new ServiceBusClient(connectionString);
            var queueSender = sbClient.CreateSender(queueName);

            // Create Local Messages
            Queue<ServiceBusMessage> messages = new();
            for (int i = 0; i < 50010; i++)
            {
                ServiceBusMessage message = new ServiceBusMessage(JsonConvert.SerializeObject(new Employee()));
                messages.Enqueue(message);
            }

            // Send Single Message
            await queueSender.SendMessageAsync(messages.Peek());

            // Batch Messages
            ServiceBusMessageBatch serviceBusMessageBatch = await queueSender.CreateMessageBatchAsync();
            Console.WriteLine(messages.Count);
            int batchCount = 0;
            while(messages.Count > 0) 
            {
                if (serviceBusMessageBatch.TryAddMessage(messages.Peek()))
                {
                    messages.Dequeue();
                }
                batchCount++;
                if(batchCount >= 1000)
                {
                    Console.WriteLine("Sending 1000...");
                    await queueSender.SendMessagesAsync(serviceBusMessageBatch);
                    batchCount = 0;
                    serviceBusMessageBatch = await queueSender.CreateMessageBatchAsync();
                }
            }

            if (batchCount > 0)
            {
                Console.WriteLine($"Sending {batchCount}...");
                await queueSender.SendMessagesAsync(serviceBusMessageBatch);
            }
            await queueSender.CloseAsync();
        }
    }
}