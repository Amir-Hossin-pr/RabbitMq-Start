using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using static System.Console;
using static System.Text.Encoding;

WriteLine("Hello From Moon");

ConnectionFactory factory = new() { HostName = "localhost" };
IConnection? connection = factory.CreateConnection();


Thread t1 = new(() =>
{
    IModel channel = connection.CreateModel();
    channel.QueueDeclare("inbox", true, false, false, null);

    EventingBasicConsumer consumer = new(channel);
    consumer.Received += Received;

    static void Received(object? sender, BasicDeliverEventArgs eventArgs)
    {
        WriteLine("Inbox Queue");
        byte[] messageBytes = eventArgs.Body.ToArray();
        var message = UTF8.GetString(messageBytes);
        WriteLine($"Earth Sended : [{message}],Routing Key : [{eventArgs.RoutingKey}], Exchange : [{eventArgs.Exchange}]");
        foreach (var header in eventArgs.BasicProperties.Headers)
        {
            WriteLine($"Header : [{header.Key}] : [{UTF8.GetString((byte[])header.Value)}]");
        }
    }

    channel.BasicConsume("inbox", false, consumer);
});

Thread t2 = new(() =>
{
    IModel channel = connection.CreateModel();
    channel.QueueDeclare("inboxMkv", true, false, false, null);

    EventingBasicConsumer consumer = new(channel);
    consumer.Received += Received;

    static void Received(object? sender, BasicDeliverEventArgs eventArgs)
    {
        WriteLine("InboxMkv Queue");
        byte[] messageBytes = eventArgs.Body.ToArray();
        var message = UTF8.GetString(messageBytes);
        WriteLine($"Earth Sended : [{message}],Routing Key : [{eventArgs.RoutingKey}], Exchange : [{eventArgs.Exchange}]");
        foreach (var header in eventArgs.BasicProperties.Headers)
        {
            WriteLine($"Header : [{header.Key}] : [{UTF8.GetString((byte[])header.Value)}]");
        }
    }

    channel.BasicConsume("inboxMkv", false, consumer);
});

t1.Start();
t2.Start();

ReadKey();