# .Net Core and RabbitMQ 
Publishing and Consuming messages with C# and RabbitMQ

Technologies:
C#
.NET 6
RabbitMQ
Editor: Visual Studio Code

Approach
Clean Architecture

## Getting started
### Installation

### Install Erlang.
Download and install the correct version of Erlang depending on the operating system that you are using.
Download and install RabbitMQ server. 

```bash
(https://www.erlang.org/downloads)
(https://www.rabbitmq.com/download.html)
```

Clone the Wonga solution from the Github repository

### SendToRabbitMQ
For the publisher, navigate to the Presentation folder and run the application.
You will be asked to enter your name.
Enter your name and press enter.

```bash
dotnet run
```

```bash
"Hello my name is, {Name}" is sent to RabbitMQ
```
A message will be sent to RabbitMQ and stored in a queue.

You can view the queue in your browser by navigating to the URL:

```bash
http://<localhost>:15672/#/queues
```

### ReceiveFromRabbitMQ
For the receiver, navigate to the Receiver folder and run the application.

```bash
dotnet run
```

The message will be retrieved from the topic that it is subscribed to on RabbitMQ. The following text will display:

```bash
"Hello {ReceivedName}, I am your father!"
```
