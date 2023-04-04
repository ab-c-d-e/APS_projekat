using TheScientistAPI.Model;

namespace TheScientistAPI.Infrastructure
{
    public interface IScientificPaperRepository:IGenericRepository<ScientificPaper>
    {
        ScientificPaper? GetById(int id, bool includeKeywords, bool includeUsers, bool includeSections, bool includeReferences, bool includeMessages);
    }
}
