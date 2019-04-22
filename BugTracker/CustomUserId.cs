using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker
{
    public class CustomUserId:IUserIdProvider
    {
        public string GetUserId(IRequest request)
        {
            var username = request.User.Identity.Name;
            return username;
        }
    }
}