namespace MyPortolioUdemy.Services
{
  /// <summary>
  /// Generic repository interface for data access operations
  /// </summary>
  public interface IRepository<T> where T : class
  {
    /// <summary>
    /// Gets all entities asynchronously
    /// </summary>
    Task<List<T>> GetAllAsync();

    /// <summary>
    /// Gets a single entity by ID asynchronously
    /// </summary>
    Task<T?> GetByIdAsync(int id);

    /// <summary>
    /// Gets entities with predicate asynchronously
    /// </summary>
    Task<List<T>> GetByConditionAsync(Func<T, bool> predicate);

    /// <summary>
    /// Gets first entity matching condition
    /// </summary>
    Task<T?> GetFirstByConditionAsync(Func<T, bool> predicate);

    /// <summary>
    /// Adds a new entity asynchronously
    /// </summary>
    Task AddAsync(T entity);

    /// <summary>
    /// Updates an existing entity asynchronously
    /// </summary>
    Task UpdateAsync(T entity);

    /// <summary>
    /// Removes an entity asynchronously
    /// </summary>
    Task RemoveAsync(T entity);

    /// <summary>
    /// Removes entity by ID asynchronously
    /// </summary>
    Task RemoveByIdAsync(int id);

    /// <summary>
    /// Saves all changes to database asynchronously
    /// </summary>
    Task SaveAsync();

    /// <summary>
    /// Gets count of all entities
    /// </summary>
    Task<int> GetCountAsync();
  }
}
