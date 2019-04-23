using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roadkill.Core.Database;

namespace Roadkill.Core.Mvc.ViewModels
{
    public class UserProfileViewModel
    {
        public Guid Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Organisation { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }

        public int OrganisationId { get; set; }

        public static UserProfileViewModel Create(User user)
        {
            if (user == null) throw new ArgumentNullException("user");
            return new UserProfileViewModel()
            {
                Id= user.Id,
                Username = user.Username,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                OrganisationId = user.orgID,
                Email = user.Email
            };
        }
    }
}
