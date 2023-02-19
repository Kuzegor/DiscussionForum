using DiscussionForum.Data.Interfaces;
using DiscussionForum.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DiscussionForum.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IEnumerable<Answer>> GetAnswersByUserAsync()
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return await _context.Answers.Where(x => x.AppUserId == currentUserId.ToString()).ToListAsync();
        }

        public async Task<IEnumerable<Question>> GetQuestionsByUserAsync()
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return await _context.Questions.Include(x => x.Answers).Where(x => x.AppUserId == currentUserId.ToString()).ToListAsync();
        }
    }
}
