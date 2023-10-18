using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.DAL.Models;

namespace YogaCenter.BackEnd.DAL.Common
{
    public class AccountResponse
    {
        public ApplicationUser User { get; set; }
        public IEnumerable<IdentityRole> Role { get; set; } = new List<IdentityRole>();
    }
}
