using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace BugTracker
{
    [Authorize]
    public class NotificationHub : Hub
    {
        public void PopupNotification(string user, string ticketName)
        {
           
            var message = ticketName + "has been modified";

            Clients.User(user).Send("Sent Sucessfully", message);
        }
    }
}