using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using static System.Console;
using static System.Text.Encoding;

WriteLine("Hello From Erth");

ConnectionFactory factory = new() { HostName = "localhost" };
IConnection? connection = factory.CreateConnection();
IModel channel = connection.CreateModel();

channel.QueueDeclare("inbox", true, false, false, null);
channel.QueueDeclare("a", true, false, false, null);
channel.QueueDeclare("b", true, false, false, null);
channel.QueueDeclare("c", true, false, false, null);
channel.ExchangeDeclare("contact", "direct", false, false, null);

channel.QueueBind("inbox", "contact", "blue", null);
channel.QueueBind("a", "contact", "blue", null);
channel.QueueBind("b", "contact", "red", null);
channel.QueueBind("c", "contact", "red", null);

string? message = " ";
string? key = "";
while (message != string.Empty)
{
    Write("Write Your Message : ");
    message = ReadLine();

    Write("Enter Your Queue Key : ");
    key = ReadLine();

    byte[] messageBytes = UTF8.GetBytes(message ?? "");
    channel.BasicPublish("contact", key, null, messageBytes);
    WriteLine($"Earth Send : [{message}]");
}