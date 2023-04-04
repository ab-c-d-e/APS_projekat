using TheScientistAPI.Model;

namespace TheScientistAPI.Infrastructure
{
    public interface ISectionRepository:IGenericRepository<Section>
    {
        Section? GetById(int id, bool includeSubsections);
    }
}
