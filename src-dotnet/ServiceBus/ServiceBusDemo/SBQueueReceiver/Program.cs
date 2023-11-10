using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;

namespace SBQueueReceiver
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
            var queueReceiver = sbClient.CreateReceiver(queueName);

            // Read Messages - For Ever
            //var messages = queueReceiver.ReceiveMessagesAsync();
            //await foreach (var message in messages)
            //{
            //    Console.WriteLine(message.Body);
            //    await queueReceiver.CompleteMessageAsync(message);
            //}

            // Get Messages variation
            //var messages = queueReceiver.ReceiveMessagesAsync();
            //await Parallel.ForEachAsync(messages, async (message, cancellationToken) =>
            //{
            //    Console.WriteLine(message.Body);
            //    await queueReceiver.CompleteMessageAsync(message);
            //});

            // Get Number of Messages
            //ServiceBusAdministrationClient client = new ServiceBusAdministrationClient(connectionString);
            //var queueProperties = (await client.GetQueueRuntimePropertiesAsync(queueName)).Value;
            //Console.WriteLine(queueProperties.TotalMessageCount);

            //var messages = await queueReceiver.ReceiveMessagesAsync((int)queueProperties.TotalMessageCount);
            //foreach (var message in messages)
            //{
            //    Console.WriteLine(message.Body);
            //    await queueReceiver.CompleteMessageAsync(message);
            //}

            ServiceBusProcessor serviceBusProcessor = sbClient.CreateProcessor(queueName);
            serviceBusProcessor.ProcessMessageAsync += MessageProcessor;
            serviceBusProcessor.ProcessErrorAsync += MessageErrorHandler;

            async Task MessageProcessor(ProcessMessageEventArgs e)
            {
                Console.WriteLine(e.Message.Body);
                await e.CompleteMessageAsync(e.Message);
            }

            Task MessageErrorHandler(ProcessErrorEventArgs e)
            {
                Console.WriteLine(e.ErrorSource);
                Console.WriteLine(e.FullyQualifiedNamespace);
                Console.WriteLine(e.EntityPath);
                Console.WriteLine(e.Exception.Message);
                return Task.CompletedTask;
            }

            await serviceBusProcessor.StartProcessingAsync();

            Console.ReadKey();
        }

        private static Task ServiceBusProcessor_ProcessErrorAsync(ProcessErrorEventArgs arg)
        {
            throw new NotImplementedException();
        }
    }
}