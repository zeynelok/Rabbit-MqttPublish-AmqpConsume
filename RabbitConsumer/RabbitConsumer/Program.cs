// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("Hello, World!");

var subs = new Subscriber();
subs.Subscribe();

Console.ReadLine();

internal class Subscriber
{
    private readonly IModel _channel;
    public Subscriber()
    {
        var connectionFactory = new ConnectionFactory();
        IList<AmqpTcpEndpoint> hosts = new List<AmqpTcpEndpoint>()
        {
            new AmqpTcpEndpoint{HostName="127.0.0.1",Port=5671},
            //new AmqpTcpEndpoint{HostName="127.0.0.1",Port=5672},
            //new AmqpTcpEndpoint{HostName="127.0.0.1",Port=5673},

        };

        var connection = connectionFactory.CreateConnection(hosts);
        Console.WriteLine("Connection Created");

        _channel = connection.CreateModel();

    }

    public void Subscribe()
    {
        var queueName = $"queue.rabbit-test";
        _channel.QueueDeclare(queueName, true, false, false);
        _channel.QueueBind(queueName, "amq.topic", "testkey");

        _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += MessageReceived;

        _channel.BasicConsume(queueName, false, consumer);
        Console.WriteLine("Waiting for message...");
    }

    void MessageReceived(object? sender, BasicDeliverEventArgs ea)
    {

        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine($"Recieved Message : {message}");
        _channel.BasicAck(ea.DeliveryTag, false);

        Thread.Sleep(1000);


    }
}