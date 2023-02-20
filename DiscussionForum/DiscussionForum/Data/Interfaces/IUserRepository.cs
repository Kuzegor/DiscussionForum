using DiscussionForum.Models;

namespace DiscussionForum.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<Answer>> GetAnswersByUserAsync();
        Task<IEnumerable<Question>> GetQuestionsByUserAsync();
        Task<IEnumerable<AppUser>> GetAllAsync();
        Task<AppUser> GetByIdAsync(string id);
        Task<AppUser> GetByIdNoTrackingAsync(string id);
        bool Update(AppUser user);
        bool Delete(AppUser user);
        bool Save();
    }
}
