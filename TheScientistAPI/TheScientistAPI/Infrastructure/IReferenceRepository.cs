
using TheScientistAPI.Model;

namespace TheScientistAPI.Infrastructure
{
    public interface IReferenceRepository: IGenericRepository<Reference>
    {
        Reference? GetByIdWithAuthors(int id);
    }
}
