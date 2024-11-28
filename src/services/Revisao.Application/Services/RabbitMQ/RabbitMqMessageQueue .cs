using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Revisao.Domain.Interfaces;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Revisao.Application.Services.RabbitMQ;

public class RabbitMqMessageQueue : IMessageQueue
{
    private readonly IModel _channel;

    public RabbitMqMessageQueue(string connectionString)
    {
        var factory = new ConnectionFactory { Uri = new Uri(connectionString) };
        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();

        // Declare a queue (se não existir)
        _channel.QueueDeclare(queue: "orders-queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
    }

    public async Task SendMessageAsync(string queueName, object message)
    {
        var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
        _channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);

        Console.WriteLine($"Message sent to queue {queueName}: {JsonConvert.SerializeObject(message)}");

        await Task.CompletedTask;
    }

    public async IAsyncEnumerable<T> ConsumeMessagesAsync<T>(string queueName, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var consumer = new EventingBasicConsumer(_channel);

        // Preparar fila para consumo
        _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

        var messageQueue = new Queue<T>();

        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(body));
            messageQueue.Enqueue(message);
        };

        // Loop de consumo assíncrono
        while (!cancellationToken.IsCancellationRequested)
        {
            while (messageQueue.Count > 0)
            {
                yield return messageQueue.Dequeue();
            }

            await Task.Delay(100); // Evitar ocupação total da CPU
        }
    }
}
