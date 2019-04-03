using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace BugTracker.Models
{
    public static class IdentityExtension
    {
        public static string GetScreenName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("ScreenName");

            return (claim != null) ? claim.Value : string.Empty;
        }
    }
}