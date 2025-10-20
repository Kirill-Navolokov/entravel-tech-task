namespace OrderProcessingService.DAL.Repos;

public interface IRepository<T>
{
    Task<T> AddAsync(T entity);

    Task<T?> GetAsync(Guid id);

    Task UpdateAsync(T entity);

    Task<IEnumerable<T>> GetByCustomer(Guid customerId);
}