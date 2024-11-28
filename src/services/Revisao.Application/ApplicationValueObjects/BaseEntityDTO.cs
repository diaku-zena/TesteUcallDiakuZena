namespace Revisao.Application.ApplicationValueObjects;

public class BaseEntityDTO
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
