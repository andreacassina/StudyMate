using Microsoft.AspNetCore.Identity;

namespace StudyMate.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string DegreeCourse { get; set; }
    }
}
