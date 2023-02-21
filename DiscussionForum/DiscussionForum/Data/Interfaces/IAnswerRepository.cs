using DiscussionForum.Models;

namespace DiscussionForum.Data.Interfaces
{
    public interface IAnswerRepository
    {
        Task<IEnumerable<Answer>> GetByQuestionId(int id);
        Task<Answer> GetByIdAsync(int id);
        bool Add(Answer answer);
        bool Update(Answer answer);
        bool Delete(Answer answer);
        bool Save();
    }
}
