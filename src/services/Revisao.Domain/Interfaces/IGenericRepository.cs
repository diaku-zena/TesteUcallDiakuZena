namespace Revisao.Domain.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(Guid id);
    //Task<T> GetByNameAsync(string name);
    Task<T> AddAsync(T entity);
    Task<bool> UpdateAsync(T entity);
    Task<bool> DeleteAsync(T entity);

}
