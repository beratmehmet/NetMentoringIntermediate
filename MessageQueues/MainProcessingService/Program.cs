using RabbitMQ.Client;
using RabbitMQ.Client.Events;

Console.WriteLine("Which type of file do you want to receive? [1].txt(default) [2].pdf");

int input = Convert.ToInt16(Console.ReadLine());
var fileType = @"txt";

switch (input)
{
    case 1:
        fileType = @"txt";
        break;
    case 2:
        fileType = @"pdf";
        break;
    default:
        Console.WriteLine("Invalid Input default file type is '.txt'");
        break;

}

var path = @"C:\ReceivedFiles";
string exchangeName = "fileExchange";
string queueName = $"{fileType}ServiceQueue";

Directory.CreateDirectory(path);

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://xwcrfqzx:U2WHFYq3meCONhrufHvIIDKwU2wV-7KK@jackal.rmq.cloudamqp.com/xwcrfqzx");

var connection = factory.CreateConnection();
var channel = connection.CreateModel();

channel.QueueDeclare(queueName, true, false, false);
channel.QueueBind(queueName, exchangeName, $"file.{fileType}");

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (sender, eventArgs) =>
{
    Console.WriteLine("File Received");
    using (FileStream fileStream = new FileStream(@$"{path}\{fileType.ToUpper()}{DateTime.Now.ToFileTime()}.{fileType}", FileMode.Create, FileAccess.Write, FileShare.None))
    {
        var message = eventArgs.Body.ToArray();
        fileStream.Write(message, 0, message.Length);
    }
    Console.WriteLine("File Transfer completed");
};

channel.BasicConsume(queueName, true, consumer);

Console.WriteLine("Press enter to exit");
Console.ReadLine();

channel.Close();
connection.Close();