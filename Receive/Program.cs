using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace Receive
{
    
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("I Can Receive Now :)");

            Program Run = new Program();
            // Excercise 1 -->
            //Run.ReceiveHello();

            // Excercise 2 -->
            Run.Worker();

            // Disse Linjer til at holde den i live SKAL være inde i metoderne
            //Console.WriteLine(Environment.NewLine + "Press Any key to close...");
            //Console.ReadKey();
        }

        private void Worker()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    string queueName = "TaskQueue";

                    channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false,
                        arguments: null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);

                        Console.WriteLine("[x] Received '{0}'", message);

                        int dots = message.Split('.').Length - 1;
                        Thread.Sleep(dots * 1000);

                        Console.WriteLine("[x] Done");
                    };

                    channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

                    Console.WriteLine(Environment.NewLine + "Press Any key to close...");
                    Console.ReadKey();
                }
            }
        }

        /// <summary>
        /// For Excercise 1
        /// </summary>
        private void ReceiveHello()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    string queueName = "Hello";

                    channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false,
                        arguments: null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);

                        Console.WriteLine("[x] Received {0}", message);
                    };

                    channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

                    Console.WriteLine(Environment.NewLine + "Press Any key to close...");
                    Console.ReadKey();
                }
            }
        }
    }
}
