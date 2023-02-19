using DiscussionForum.Models;

namespace DiscussionForum.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<Answer>> GetAnswersByUserAsync();
        Task<IEnumerable<Question>> GetQuestionsByUserAsync();


    }
}
