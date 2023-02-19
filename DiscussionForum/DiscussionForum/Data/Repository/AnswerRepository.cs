using DiscussionForum.Data.Interfaces;
using DiscussionForum.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscussionForum.Data.Repository
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly AppDbContext _context;
        public AnswerRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool Add(Answer answer)
        {
            _context.Add(answer);
            return Save();
        }

        public bool Delete(Answer answer)
        {
            _context.Remove(answer);
            return Save();
        }

        public bool Save()
        {
            int savedChanges = _context.SaveChanges();
            return savedChanges > 0 ? true : false;
        }

        public bool Update(Answer answer)
        {
            _context.Update(answer);
            return Save();
        }
    }
}
