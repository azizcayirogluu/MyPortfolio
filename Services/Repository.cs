using Microsoft.EntityFrameworkCore;
using MyPortolioUdemy.DAL.Context;

namespace MyPortolioUdemy.Services
{
  /// <summary>
  /// Generic repository implementation for data access operations
  /// </summary>
  public class Repository<T> : IRepository<T> where T : class
  {
    private readonly MyPortfolioContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(MyPortfolioContext context)
    {
      _context = context ?? throw new ArgumentNullException(nameof(context));
      _dbSet = context.Set<T>();
    }

    /// <summary>
    /// Gets all entities asynchronously
    /// </summary>
    public async Task<List<T>> GetAllAsync()
    {
      try
      {
        return await _dbSet.AsNoTracking().ToListAsync();
      }
      catch (Exception ex)
      {
        throw new Exception($"Error retrieving all {typeof(T).Name} entities: {ex.Message}", ex);
      }
    }

    /// <summary>
    /// Gets a single entity by ID asynchronously
    /// </summary>
    public async Task<T?> GetByIdAsync(int id)
    {
      try
      {
        if (id <= 0)
          throw new ArgumentException("ID must be greater than 0", nameof(id));

        return await _dbSet.FindAsync(id);
      }
      catch (Exception ex)
      {
        throw new Exception($"Error retrieving {typeof(T).Name} with ID {id}: {ex.Message}", ex);
      }
    }

    /// <summary>
    /// Gets entities with predicate asynchronously
    /// </summary>
    public async Task<List<T>> GetByConditionAsync(Func<T, bool> predicate)
    {
      try
      {
        if (predicate == null)
          throw new ArgumentNullException(nameof(predicate));

        return await Task.FromResult(_dbSet.Where(predicate).ToList());
      }
      catch (Exception ex)
      {
        throw new Exception($"Error retrieving {typeof(T).Name} entities by condition: {ex.Message}", ex);
      }
    }

    /// <summary>
    /// Gets first entity matching condition
    /// </summary>
    public async Task<T?> GetFirstByConditionAsync(Func<T, bool> predicate)
    {
      try
      {
        if (predicate == null)
          throw new ArgumentNullException(nameof(predicate));

        return await Task.FromResult(_dbSet.FirstOrDefault(predicate));
      }
      catch (Exception ex)
      {
        throw new Exception($"Error retrieving first {typeof(T).Name} by condition: {ex.Message}", ex);
      }
    }

    /// <summary>
    /// Adds a new entity asynchronously
    /// </summary>
    public async Task AddAsync(T entity)
    {
      try
      {
        if (entity == null)
          throw new ArgumentNullException(nameof(entity));

        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
      }
      catch (Exception ex)
      {
        throw new Exception($"Error adding {typeof(T).Name}: {ex.Message}", ex);
      }
    }

    /// <summary>
    /// Updates an existing entity asynchronously
    /// </summary>
    public async Task UpdateAsync(T entity)
    {
      try
      {
        if (entity == null)
          throw new ArgumentNullException(nameof(entity));

        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
      }
      catch (Exception ex)
      {
        throw new Exception($"Error updating {typeof(T).Name}: {ex.Message}", ex);
      }
    }

    /// <summary>
    /// Removes an entity asynchronously
    /// </summary>
    public async Task RemoveAsync(T entity)
    {
      try
      {
        if (entity == null)
          throw new ArgumentNullException(nameof(entity));

        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
      }
      catch (Exception ex)
      {
        throw new Exception($"Error removing {typeof(T).Name}: {ex.Message}", ex);
      }
    }

    /// <summary>
    /// Removes entity by ID asynchronously
    /// </summary>
    public async Task RemoveByIdAsync(int id)
    {
      try
      {
        if (id <= 0)
          throw new ArgumentException("ID must be greater than 0", nameof(id));

        var entity = await _dbSet.FindAsync(id);
        if (entity != null)
        {
          _dbSet.Remove(entity);
          await _context.SaveChangesAsync();
        }
      }
      catch (Exception ex)
      {
        throw new Exception($"Error removing {typeof(T).Name} with ID {id}: {ex.Message}", ex);
      }
    }

    /// <summary>
    /// Saves all changes to database asynchronously
    /// </summary>
    public async Task SaveAsync()
    {
      try
      {
        await _context.SaveChangesAsync();
      }
      catch (Exception ex)
      {
        throw new Exception($"Error saving changes: {ex.Message}", ex);
      }
    }

    /// <summary>
    /// Gets count of all entities
    /// </summary>
    public async Task<int> GetCountAsync()
    {
      try
      {
        return await _dbSet.CountAsync();
      }
      catch (Exception ex)
      {
        throw new Exception($"Error counting {typeof(T).Name} entities: {ex.Message}", ex);
      }
    }
  }
}
