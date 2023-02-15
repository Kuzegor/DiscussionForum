using DiscussionForum.Models;

namespace DiscussionForum.Data.Interfaces
{
    public interface IAnswerRepository
    {
        Task<IEnumerable<Answer>> GetByUserIdAsync(string appUserId);
        bool Add(Answer answer);
        bool Update(Answer answer);
        bool Delete(Answer answer);
        bool Save();
    }
}
