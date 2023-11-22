
using RabbitMQ.Client;

string path = @"C:\test";
string exchangeName = "fileExchange";

Directory.CreateDirectory(path);

using var watcher = new FileSystemWatcher(path);

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://xwcrfqzx:U2WHFYq3meCONhrufHvIIDKwU2wV-7KK@jackal.rmq.cloudamqp.com/xwcrfqzx");

var connection = factory.CreateConnection();
var channel = connection.CreateModel();

channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, true);

watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;

watcher.IncludeSubdirectories = false;
watcher.EnableRaisingEvents = true;

watcher.Created += (object sender, FileSystemEventArgs e) =>
{
    Console.WriteLine($"Detected: {e.FullPath}");
    using (FileStream fStream = new FileStream(e.FullPath, FileMode.Open))
    {
        byte[] byteArray = new byte[1024];

        fStream.Read(byteArray, 0, byteArray.Length);
        channel.BasicPublish(exchangeName, $"file{Path.GetExtension(e.FullPath)}", null, byteArray);
    }
};

Console.WriteLine("Press enter to exit.");
Console.ReadLine();

channel.Close();
connection.Close();