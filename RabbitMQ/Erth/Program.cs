using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using static System.Console;
using static System.Text.Encoding;

WriteLine("Hello From Earth");

ConnectionFactory factory = new() { HostName = "localhost" };
IConnection? connection = factory.CreateConnection();
IModel channel = connection.CreateModel();

Dictionary<string, object> headers = new()
{
    ["x-match"] = "any",
    ["format"] = "bmp"
};

Dictionary<string, object> headers2 = new()
{
    ["x-match"] = "any",
    ["format"] = "mkv"
};


channel.ExchangeDeclare("format", "headers", true, false, null);
channel.QueueDeclare("inbox", true, false, false, null);
channel.QueueDeclare("inboxMkv", true, false, false, null);

channel.QueueBind("inbox", "format", "", headers);
channel.QueueBind("inboxMkv", "format", "", headers2);

string? message = " ";
while (message != string.Empty)
{
    Write("Write Your Message : ");
    message = ReadLine();
    Write("What is your format? : ");
    string? format = ReadLine();
   
    IBasicProperties? properties = channel.CreateBasicProperties();
    properties.Headers = new Dictionary<string, object>
    {
        ["time"] = DateTime.Now.ToShortTimeString(),
        ["format"] = format ?? "",
    };

    byte[] messageBytes = UTF8.GetBytes(message ?? "");
    channel.BasicPublish("format", format, properties, messageBytes);
    WriteLine($"Earth Send : [{message}]");
}