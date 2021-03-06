﻿using Hyushik_TournMan_Web.Filters;
using System.Web;
using System.Web.Mvc;

namespace Hyushik_TournMan_Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new InitializeSimpleMembershipAttribute());
        }
    }
}