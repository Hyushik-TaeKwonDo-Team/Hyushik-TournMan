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
    [Hyushik_TournMan_Web.Filters.InitializeSimpleMembership]
    public class BaseController : Controller
    {

        protected ActionResult RedirectToHome()
        {
            return RedirectToAction("Index", "Home");
        }

        protected ActionResult RedirectToActiveTourn(long tournId)
        {
            return RedirectToAction("Index", "ActiveTournament", new { tournId = tournId });
        }

        protected ActionResult RedirectToRing(long ringId)
        {
            return RedirectToAction("Index", "Ring", new { ringId = ringId });
        }

        protected void AddNotifications(IDictionary<string, string> notificationsToAdd)
        {
            IDictionary<string,string> currentNotifications =  
                (IDictionary<string,string>) TempData[WebConstants.Parameters.NOTIFICATIONS] ?? new Dictionary<string, string>();
 
            foreach( var notification in notificationsToAdd.Keys ){
                currentNotifications[notification] = notificationsToAdd[notification];
            }

            TempData[WebConstants.Parameters.NOTIFICATIONS] = currentNotifications;
        }

        protected void AddSingleNotification(string message, string cssClass)
        {
            IDictionary<string, string> currentNotifications =
                (IDictionary<string, string>)TempData[WebConstants.Parameters.NOTIFICATIONS] ?? new Dictionary<string, string>();
            currentNotifications[message] = cssClass;

            TempData[WebConstants.Parameters.NOTIFICATIONS] = currentNotifications;
        }

        protected void AddSucessNotification(string message)
        {
            AddSingleNotification(message, WebConstants.CssClasses.Notifications.SUCCESS);
        }

        protected void AddErrorNotification(string message)
        {
            AddSingleNotification(message, WebConstants.CssClasses.Notifications.ERROR);
        }

        protected void AddWarningNotification(string message)
        {
            AddSingleNotification(message, WebConstants.CssClasses.Notifications.WARNING);
        }

        protected void AddInfoNotification(string message)
        {
            AddSingleNotification(message, WebConstants.CssClasses.Notifications.INFO);
        }

    }
}
