using System.ComponentModel.DataAnnotations;

namespace DiscussionForum.Models
{
    public class AppUser
    {
        [Key]
        public string? Id { get; set; }
        public ICollection<Answer>? Answers { get; set; }
        public ICollection<Question>? Questions { get; set; }
    }
}
