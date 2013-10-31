using Hyushik_TournMan_Web.Classes.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Web;
using System.Web.Mvc;

namespace Hyushik_TournMan_Web.Controllers
{
    public class BaseController : Controller
    {
        protected void AddNotifications(IDictionary<string, string> notificationsToAdd)
        {
            IDictionary<string,string> currentNotifications =  
                (IDictionary<string,string>) TempData[WebConstants.Parameters.NOTIFICATIONS] ?? new Dictionary<string, string>();
 
            foreach( var notification in notificationsToAdd.Keys ){
                currentNotifications[notification] = notificationsToAdd[notification];
            }

            TempData[WebConstants.Parameters.NOTIFICATIONS] = currentNotifications;
        }

    }
}
