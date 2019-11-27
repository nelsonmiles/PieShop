using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Shop.Auth
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime Birthdate { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; } = new List<IdentityUserClaim<string>>();
        }
}