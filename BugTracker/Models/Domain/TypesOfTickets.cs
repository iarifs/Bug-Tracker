using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models.Domain
{
    public enum TypesOfTickets
    {
        Bug,
        Feature,
        Database,
        Support
    }

    public enum TypesOfTicektPriorites
    {
        Low,
        Medium,
        High
    }

    public enum TypesOfTicketStatuses
    {
        Open,
        Resolved,
        Rejected,
    }
}