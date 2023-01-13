namespace TheScientistAPI.Infrastructure
{
    public interface IGenericRepository<T> where T: class
    {
        Task<List<T>> All();
        Task<T> GetById(int id);
        Task<bool> Add(T entity);
        Task<bool> Delete(int id);
        Task<bool> Upsert(T entity);
    }
}
