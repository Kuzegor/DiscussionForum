using DiscussionForum.Models;

namespace DiscussionForum.Data.Interfaces
{
    public interface IAnswerRepository
    {
        bool Add(Answer answer);
        bool Update(Answer answer);
        bool Delete(Answer answer);
        bool Save();
    }
}
