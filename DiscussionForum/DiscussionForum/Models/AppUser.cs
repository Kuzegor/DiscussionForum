using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscussionForum.Models
{
    public class AppUser : IdentityUser
    {
        public string? Image { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        public ICollection<Answer>? Answers { get; set; }
        public ICollection<Question>? Questions { get; set; }
    }
}
