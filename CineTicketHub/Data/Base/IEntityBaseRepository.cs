namespace CineTicketHub.Models.Base;

public interface IEntityBaseRepository<E> where E : class, IEntityBase, new()
{
    Task<IEnumerable<E>> GetAllAsync();
    Task<E> GetByIdAsync(int id);
    Task AddAsync(E entity);
    Task UpdateAsync(int id, E entity);
    Task<E> DeleteAsync(int id);
}