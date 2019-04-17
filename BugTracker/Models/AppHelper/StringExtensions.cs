using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace BugTracker.Models
{
    public static class StringExtensions
    {
        public static string ToControllerName(this string controllerName) //TicketController
        {
            return controllerName.Replace("Controller", string.Empty);
        }
    }
}