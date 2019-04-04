using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Models.ViewModels
{

    public class UserListViewModel
    {
        public string Id { get; set; }

        public string ScreenName { get; set; }

        public string UserName { get; set; }

        public List<string>Roles { get; set; }


        public UserListViewModel()
        {
            Roles = new List<string>();

        }
    }
}