using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Wasla.Enums;

namespace Wasla.DAL.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public UserType UserType { get; set; }
    }
}
