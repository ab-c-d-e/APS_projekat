using TheScientistAPI.Data;
using TheScientistAPI.Model;
using TheScientistAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace TheScientistAPI.Service
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ScientistContext context, ILogger logger) : base(context, logger) { }

        public async Task<bool> Delete(string id)
        {
            try
            {
                var existing = await _dbSet.Where(u => u.Id == id)
                    .FirstOrDefaultAsync();

                if (existing != null)
                {
                    _dbSet.Remove(existing);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo}, Delete method error", typeof(UserRepository));
                return false;
            }
        }

        public async Task<User> GetById(string id)
        {
            return
                await _dbSet.FindAsync(id) ??
                throw new Exception("Entity with this key does not exist");
        }

        public override async Task<bool> Upsert(User entity)
        {
            try
            {
                var existing = await _dbSet.Where(u => u.Id == entity.Id)
                    .FirstOrDefaultAsync();

                if (existing == null)
                    return await Add(entity);

                existing.Email = entity.Email;
                existing.UserName = entity.UserName;
                existing.PasswordHash = entity.PasswordHash;
                existing.Name = entity.Name;
                existing.LastName = entity.LastName;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo}, Upsert method error", typeof(UserRepository));
                return false;
            }
        }

        //public override async Task<bool> Delete(String id)
        //{
        //    try
        //    {
        //        var existing = await _dbSet.Where(u => u.Id == id)
        //            .FirstOrDefaultAsync();

        //        if (existing != null)
        //        {
        //            _dbSet.Remove(existing);
        //            return true;
        //        }

        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "{Repo}, Delete method error", typeof(UserRepository));
        //        return false;
        //    }
        //}
    }
}
