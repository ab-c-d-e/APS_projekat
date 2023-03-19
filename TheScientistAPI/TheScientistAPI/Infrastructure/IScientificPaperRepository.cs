using TheScientistAPI.Model;

namespace TheScientistAPI.Infrastructure
{
    public interface IScientificPaperRepository:IGenericRepository<ScientificPaper>
    {
        ScientificPaper GetById(int id, bool includeRelatedEntities);
    }
}
