using Hyushik_TournMan_Common.Models;
using System.Collections.Generic;

namespace Hyushik_TournMan_Web.Classes.ViewModels
{
    public class UserRolesViewModel
    {
        public IList<UserProfile> Users {get; set;}
        public IDictionary<string, string[]> UserNameToRoles { get; set; }
    }
}