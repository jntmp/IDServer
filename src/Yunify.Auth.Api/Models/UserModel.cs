using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yunify.Auth.Api.Models
{
    public class UserModel
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Serial { get; set; }

        public UserModel(string userId, string firstName, string lastName, string serial)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Serial = serial;
        }
    }
}
