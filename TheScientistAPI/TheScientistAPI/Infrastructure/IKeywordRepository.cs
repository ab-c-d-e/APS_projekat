using TheScientistAPI.Model;

namespace TheScientistAPI.Infrastructure
{
    public interface IKeywordRepository:IGenericRepository<Keyword>
    {
        Keyword? GetByName(string name);
    }
}
