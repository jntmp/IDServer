using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Yunify.Auth.Server.Model
{
    public class UserModel : IdentityUser<string>
    {
        [Key]
        public string UserId { get; set; }
        public string Password { get; set; }
        public string Serial { get; set; }
    }

    public class UserRole : IdentityRole
    {
        
    }
}
