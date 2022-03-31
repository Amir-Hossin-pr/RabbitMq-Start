using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using static System.Console;
using static System.Text.Encoding;

WriteLine("Hello From Erth");

ConnectionFactory factory = new() { HostName = "localhost" };
IConnection? connection = factory.CreateConnection();
IModel channel = connection.CreateModel();

channel.QueueDeclare("inbox", true, false, false, null);

string? message = " ";
while (message != string.Empty)
{
    Write("Write Your Message : ");
    message = ReadLine();

    byte[] messageBytes = UTF8.GetBytes(message ?? "");
    channel.BasicPublish("", "inbox", null, messageBytes);
    WriteLine($"Erth Send : [{message}]");
}