using SafeResult;

namespace SimpleBlog.Data.Repositories.Abstract;

public interface IRepository<T>
    where T : class, new()
{
    Task<List<SafeResult<T>>> GetAll();
    Task<SafeResult<T>> GetById(int id);
    Task Add(T entity);
    Task Update(T entity);
    Task Delete(T entity);
}