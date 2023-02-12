using DiscussionForum.Data.Interfaces;
using DiscussionForum.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscussionForum.Data.Repository
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly AppDbContext _context;
        public QuestionRepository(AppDbContext context)
        {
            _context = context;
        }
        public bool Add(Question question)
        {
            _context.Add(question);
            return Save();
        }

        public bool Delete(Question question)
        {
            _context.Remove(question);
            return Save();
        }

        public async Task<IEnumerable<Question>> GetAll()
        {
            return await _context.Questions.ToListAsync();
        }

        public async Task<Question> GetById(int id)
        {
            return await _context.Questions.Include(x => x.Answers).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Question>> GetByUserId(string appUserId)
        {
            return await _context.Questions.Where(x => x.AppUserId == appUserId).ToListAsync(); 
        }

        public bool Save()
        {
            int savedChanges = _context.SaveChanges();
            return savedChanges > 0 ? true : false;
        }

        public bool Update(Question question)
        {
            _context.Update(question);
            return Save();
        }
    }
}
