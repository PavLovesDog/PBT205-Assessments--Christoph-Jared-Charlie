using System;
using System.Text;
using RabbitMQ.Client;

//START RABBITMQ: docker run -d --hostname <my-rabbit> --name <some name> rabbitmq:3
//                              name of server ^    name of container ^

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

// create message
var message = "This is mesaage";

//encode message
var encodedMessage = Encoding.UTF8.GetBytes(message);

//publish message
channel.BasicPublish("", "LetterBox", null, encodedMessage);


Console.WriteLine($"Published Message: {message}");

//so program doesn't exit
Console.ReadKey();

