using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscussionForum.Models
{
    public class Answer
    {
        [Key]
        public int Id { get; set; }
        public string? Description { get; set; }
        [ForeignKey("AppUser")]
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        public Question? Question { get; set; }
    }
}
