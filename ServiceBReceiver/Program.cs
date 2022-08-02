using RabbitMQ.Client;
using RabbitMQ.Client.Events;


try
{
        string receivedName= "";

        var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
            var connection = factory.CreateConnection();
        var channel = connection.CreateModel();

        channel.QueueDeclare("comunicateQueue", true, false, false);
        channel.QueueBind("comunicateQueue", "communicationExchange", "communicate.Name");

        //channel.ExchangeDeclare("communicationExchange", ExchangeType.Direct, true);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (sender, eventArgs) =>
        {
            var msg = System.Text.Encoding.UTF8.GetString(eventArgs.Body.ToArray());

            receivedName = msg.Split(' ').Last();
            Console.WriteLine($"Hello {receivedName}, I am your father! ");
        };


        channel.BasicConsume("comunicateQueue", true, consumer);

        
        Console.ReadLine();

        channel.Close();
        connection.Close();
}
catch (Exception ex)
{
    throw ex;
}