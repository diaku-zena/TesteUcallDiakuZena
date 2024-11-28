
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Revisao.Domain.Entities;
using Revisao.Domain.Interfaces;
using Revisao.Domain.Shared.Enums;
using Microsoft.Extensions.Hosting;

using global::RabbitMQ.Client.Events;
using global::RabbitMQ.Client;
using global::Revisao.Domain.Interfaces;
using global::Revisao.Domain.Shared.Enums;

namespace Revisao.Application.Services.RabbitMQ;

public class OrderMessageConsumer : BackgroundService
{
    private readonly IModel _channel;
    private readonly IConnection _connection;

    private readonly IMessageQueue _messageQueue;
    private readonly IOrderRepository _orderRepository;

    public OrderMessageConsumer(IMessageQueue messageQueue, IOrderRepository orderRepository)
    {
        _messageQueue = messageQueue;
        _orderRepository = orderRepository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Consumir mensagens da fila "orders-queue"
        await foreach (var message in _messageQueue.ConsumeMessagesAsync<OrderMessage>(queueName: "orders-queue", cancellationToken: stoppingToken))
        {
            var orderId = message.OrderId;

            // Processar o pedido (por exemplo, atualizar o status)
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order != null)
            {
                order.Status = OrderStatusEnum.Processing;
                await _orderRepository.UpdateOrderStatusAsync(orderId, OrderStatusEnum.Processing);

                // Simulação de processamento
                await Task.Delay(10000);

                order.Status = OrderStatusEnum.Completed;
                await _orderRepository.UpdateOrderStatusAsync(orderId, OrderStatusEnum.Completed);
            }
        }
    }
    public async Task StartConsuming(CancellationToken cancellationToken)
    {
        await foreach (var message in _messageQueue.ConsumeMessagesAsync<OrderMessage>("orders-queue", cancellationToken))
        {
            // Processar a mensagem
            await _orderRepository.UpdateOrderStatusAsync(message.OrderId, OrderStatusEnum.Processing);
            Console.WriteLine($"Order {message.OrderId} is now Processing.");
        }
    }

    public void StartConsuming()
    {
        // Configura o consumidor da fila
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            // Deserializa a mensagem
            var orderMessage = JsonConvert.DeserializeObject<OrderMessage>(message);

            // Processa a mensagem (atualiza o status do pedido)
            await ProcessOrderAsync(orderMessage.OrderId);
        };

        // Começa a consumir mensagens da fila "orders-queue"
        _channel.BasicConsume(queue: "orders-queue", autoAck: true, consumer: consumer);
    }

    private async Task ProcessOrderAsync(Guid orderId)
    {
        var order = await _orderRepository.GetOrderByIdAsync(orderId);

        if (order == null)
        {
            Console.WriteLine("Pedido não encontrado");
            return;
        }

        // Atualiza o status para 'em processamento'
        order.Status = OrderStatusEnum.Processing;
        await _orderRepository.UpdateOrderStatusAsync(orderId, OrderStatusEnum.Processing);

        // Simula um delay de processamento
        Console.WriteLine("Processando o pedido...");
        await Task.Delay(5000); // Simulando o tempo de processamento

        // Atualiza o status para 'concluído'
        order.Status = OrderStatusEnum.Completed;
        await _orderRepository.UpdateOrderStatusAsync(orderId, OrderStatusEnum.Completed);

        Console.WriteLine("Pedido processado com sucesso");
    }

    public void StopConsuming()
    {
        _channel.Close();
        _connection.Close();
    }

}

// Classe de Mensagem para a Deserialização
public class OrderMessage
{
    public Guid OrderId { get; set; }
}

