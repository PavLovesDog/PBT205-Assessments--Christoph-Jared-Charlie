using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

//create connection factory
var factory = new ConnectionFactory{ HostName = "localhost" };

//create a connection
using var connection = factory.CreateConnection();

//create channel
using var channel = connection.CreateModel();

//declare queue to use
channel.QueueDeclare(
        queue: "LetterBox", 
        durable:false, 
        exclusive: false, 
        autoDelete: false, 
        arguments: null);

//consumer to consume events from channel
var consumer = new EventingBasicConsumer(channel);

//define callback thats called when message is recieved
consumer.Received += (model, ea) =>
//when message received, what do we wanna do?
{
    var body = ea.Body.ToArray(); //get body of message
    var message = Encoding.UTF8.GetString(body); // decode encoded body
    Console.WriteLine($"Message Received: {message}"); // print out message received
};

//consume messages
channel.BasicConsume(queue: "LetterBox", autoAck: true, consumer: consumer);

//so program doesn't exit
Console.ReadKey();

