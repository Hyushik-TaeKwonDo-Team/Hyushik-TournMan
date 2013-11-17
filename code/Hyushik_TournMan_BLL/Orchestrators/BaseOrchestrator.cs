﻿using Hyushik_TournMan_Common.Models;
using Hyushik_TournMan_DAL.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyushik_TournMan_BLL.Orchestrators
{
    public class BaseOrchestrator
    {
        private UsersContext _usersContext = new UsersContext();

        public IEnumerable<UserProfile> GetUsers(){
            return _usersContext.UserProfiles;
        }
    }
}