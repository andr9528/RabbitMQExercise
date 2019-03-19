using System;
using RabbitMQ.Client;
using System.Text;

namespace Send
{
    
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("I Can Send Now :)");

            Program Run = new Program();
            // Excercise 1 -->
            //Run.SendHello();

            // Excercise 2 -->
            Run.NewTask(args);

            // Disse Linjer til at holde den i live SKAL være inde i metoderne
            //Console.WriteLine(Environment.NewLine + "Press Any key to close...");
            //Console.ReadKey();
        }

        private void NewTask(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    string queueName = "TaskQueue";

                    channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false,
                        arguments: null);

                    var message = GetMessage(args);
                    var body = Encoding.UTF8.GetBytes(message);

                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;

                    channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: properties, body: body);
                    Console.WriteLine("[x] Sent {0}", message);

                    
                }
            }
        }

        /// <summary>
        /// For Excercise 1
        /// </summary>
        private void SendHello()
        {
            var factory = new ConnectionFactory() {HostName = "localhost"};
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    string queueName = "Hello";

                    channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false,
                        arguments: null);

                    var message = "Hello World!";
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
                    Console.WriteLine("[x] Sent {0}", message);

                    Console.WriteLine(Environment.NewLine + "Press Any key to close...");
                    Console.ReadKey();
                }
            }
        }

        private string GetMessage(string[] args)
        {
            return ((args.Length > 0) ? string.Join(" ", args) : "Hello World!");
        }
    }
}
