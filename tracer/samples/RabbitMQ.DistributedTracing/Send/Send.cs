﻿using System;
using System.Collections.Generic;
using System.Text;
using Datadog.Trace;
using RabbitMQ.Client;

namespace Send
{
    class Send
    {
        // This small application follows the RabbitMQ tutorial at https://www.rabbitmq.com/tutorials/tutorial-one-dotnet.html
        public static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                // Note that this custom span is not necessary, it is here just to demonstrate that the trace id propagation works.
                using (var scope = Tracer.Instance.StartActive("parent.span"))
                {
                    var message = "Hello World from trace id: " + scope.Span.TraceId ;
                    var body = Encoding.UTF8.GetBytes(message);

                    // Create BasicProperties and a Headers dictionary to store header information
                    var properties = channel.CreateBasicProperties();
                    properties.Headers = new Dictionary<string, object>();

                    // Publish message
                    channel.BasicPublish(exchange: "",
                        routingKey: "hello",
                        basicProperties: properties, // Pass the properties with the message
                        body: body);

                    // Log message and properties to screen
                    Console.WriteLine(" [x] Sent {0}", message);
                }
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
