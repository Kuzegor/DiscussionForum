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

        public async Task<IEnumerable<AppUser>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<AppUser> GetByIdAsync(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<IEnumerable<Answer>> GetAnswersByUserAsync()
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return await _context.Answers.Where(x => x.AppUserId == currentUserId.ToString()).ToListAsync();
        }

        public async Task<IEnumerable<Question>> GetQuestionsByUserAsync()
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return await _context.Questions.Include(x => x.Answers).Include(x => x.AppUser).Where(x => x.AppUserId == currentUserId.ToString()).ToListAsync();
        }

        public bool Update(AppUser user)
        {
            _context.Update(user);
            return Save();
        }

        public bool Delete(AppUser user)
        {
            _context.Remove(user);
            return Save();
        }

        public bool Save()
        {
            int savedChanges = _context.SaveChanges();
            return savedChanges > 0 ? true : false;
        }

        public async Task<AppUser> GetByIdNoTrackingAsync(string id)
        {
            var appUser = await _context.Users.Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();
            return appUser;
        }
    }
}
