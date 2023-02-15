using DiscussionForum.Models;

namespace DiscussionForum.Data.Interfaces
{
    public interface IQuestionRepository
    {
        Task<IEnumerable<Question>> GetAllAsync();
        Task<Question> GetByIdAsync(int id);
        Task<IEnumerable<Question>> GetByUserIdAsync(string appUserId);
        bool Add(Question question);
        bool Update(Question question);
        bool Delete(Question question);
        bool Save();

    }
}
