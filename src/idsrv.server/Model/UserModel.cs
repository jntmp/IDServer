using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace idsrv.server.Model
{
    public class UserModel 
    {
        [Key]
        public string UserId { get; set; }
        [Key]
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Password { get; set; }
        public int AccessFailedCount { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public string Serial { get; set; }
    }

    public class UserRole 
    {
        
    }
}
