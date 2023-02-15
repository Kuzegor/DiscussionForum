using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DiscussionForum.Models
{
    public class AppUser : IdentityUser
    {
        public ICollection<Answer>? Answers { get; set; }
        public ICollection<Question>? Questions { get; set; }
    }
}
