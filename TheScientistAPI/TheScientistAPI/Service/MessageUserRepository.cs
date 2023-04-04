using TheScientistAPI.Data;
using TheScientistAPI.Infrastructure;
using TheScientistAPI.Model;

namespace TheScientistAPI.Service
{
    public class MessageUserRepository : GenericRepository<MessageUser>, IMessageUserRepository
    {
        public MessageUserRepository(ScientistContext context, ILogger logger) : base(context, logger)
        {
        }
        public List<MessageUser> GetByUserAndPaper(string userId, int paperId)
        {
            var paper = _context.ScientificPapers.FirstOrDefault(sP=>sP.Id==paperId);
            var query = _context.Set<MessageUser>().AsQueryable();
            var messageUsers = query.Where(mU => (mU.User.Id == userId)).ToList();
            var messages = new List<MessageUser>();
            foreach (var message in messageUsers)
                if (paper.Messages.Contains(message.Message)) messages.Add(message);
            return messages;
        }
    }
}
