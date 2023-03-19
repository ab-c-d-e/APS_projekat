namespace TheScientistAPI.Infrastructure
{
    public interface IGenericRepository<T> where T: class
    {
        void Add(T entity);
        void Remove(T entity);
        void Update(T entity);
        T GetById(int id);
        IEnumerable<T> GetAll();
    }
}
