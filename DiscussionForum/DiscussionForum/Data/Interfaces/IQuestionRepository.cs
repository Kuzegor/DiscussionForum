using DiscussionForum.Models;

namespace DiscussionForum.Data.Interfaces
{
    public interface IQuestionRepository
    {
        Task<IEnumerable<Question>> GetAll();
        Task<Question> GetById(int id);
        Task<IEnumerable<Question>> GetByUserId(string appUserId);
        bool Add(Question question);
        bool Update(Question question);
        bool Delete(Question question);
        bool Save();

    }
}
