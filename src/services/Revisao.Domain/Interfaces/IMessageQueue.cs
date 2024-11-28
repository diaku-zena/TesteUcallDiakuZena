
namespace Revisao.Domain.Interfaces;

public interface IMessageQueue
{
    Task SendMessageAsync(string queueName, object message);
    IAsyncEnumerable<T> ConsumeMessagesAsync<T>(string queueName, CancellationToken cancellationToken);
}

