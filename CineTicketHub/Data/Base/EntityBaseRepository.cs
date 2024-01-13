using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CineTicketHub.Models.Base;

public class EntityBaseRepository<E> : IEntityBaseRepository<E> where E: class, IEntityBase, new()
{
    private readonly CineTicketHubContext _context;

    public EntityBaseRepository(CineTicketHubContext context)
    {
        this._context = context;
    }
    
    public async Task<IEnumerable<E>> GetAllAsync()
    {
        var result = await _context.Set<E>().ToListAsync();
        return result;
    }

    public async Task<E> GetByIdAsync(int id)
    {
        var result = await _context.Set<E>().FirstOrDefaultAsync(e => e.Id == id);
        return result;
    }

    public async Task AddAsync(E entity)
    {
        await _context.Set<E>().AddAsync(entity);
    }

    public async Task UpdateAsync(int id, E entity)
    {
        EntityEntry entityEntry = _context.Entry<E>(entity);
        entityEntry.State = EntityState.Modified;
    }

    public async Task<E> DeleteAsync(int id)
    {
        var entity = await _context.Set<E>().FirstOrDefaultAsync(e => e.Id == id);
        EntityEntry entityEntry = _context.Entry<E>(entity);
        entityEntry.State = EntityState.Deleted;
        return entity;
    }
}