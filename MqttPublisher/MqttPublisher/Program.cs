// See https://aka.ms/new-console-template for more information
using MQTTnet;
using MQTTnet.Client;


await Publish_Application_Message();

Console.ReadLine();


static async Task Publish_Application_Message()
{

    var mqttFactory = new MqttFactory();

    using (var mqttClient = mqttFactory.CreateMqttClient())
    {
        var mqttClientOptions = new MqttClientOptionsBuilder()
            //.WithWillQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce).WithCleanSession(false)
            .WithTcpServer("127.0.0.1",1883)
            .Build();

        await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

        var value = 0;
        while (true)
        {
            var applicationMessage = new MqttApplicationMessageBuilder()
           .WithTopic("testkey")
           .WithPayload($"{value}")
           .Build();
            await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);
            Console.WriteLine("MQTT application message is published. Value: " + value);
            value++;
            Thread.Sleep(5000);

        }


        await mqttClient.DisconnectAsync();

        Console.WriteLine("MQTT application message is published.");
    }
}