using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using static System.Console;
using static System.Text.Encoding;

WriteLine("Hello From Moon");

ConnectionFactory factory = new() { HostName = "localhost" };
IConnection? connection = factory.CreateConnection();
IModel channel = connection.CreateModel();

channel.QueueDeclare("inbox", true, false, false, null);

EventingBasicConsumer consumer = new(channel);
consumer.Received += Received;

static void Received(object? sender, BasicDeliverEventArgs eventArgs)
{
    byte[] messageBytes = eventArgs.Body.ToArray();
    var message = UTF8.GetString(messageBytes);

    WriteLine($"Earth Sended : [{message}],Exchange is : [{eventArgs.Exchange}], Routing Key is : [{eventArgs.RoutingKey}]");
}

channel.BasicConsume("inbox", false, consumer);

ReadKey();