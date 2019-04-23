using BugTracker.Models.Domain;
using BugTracker.Models.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BugTracker.Models.AppHelper
{
    public class TrackChanges
    {
        private Ticket Model { get; set; }
        private CreateTicketsViewModel FormData { get; set; }
        private ApplicationDbContext Db { get; set; }

        public TrackChanges(ApplicationDbContext context, Ticket ticket, CreateTicketsViewModel dataFromView)
        {
            Db = context;
            Model = ticket;
            FormData = dataFromView;
        }

        public bool IsItChanged()
        {
            if (Model.Title != FormData.Title ||
                Model.Description != FormData.Description ||
                Model.TicketPriorityId != FormData.TicketPriorityId ||
                Model.TicketStatusId != FormData.TicketStatusId ||
                Model.TicketTypeId != FormData.TicketTypeId) return true;
            else return false;
        }

        public List<History> ModifiedValues()
        {
            var Histories = new List<History>();

            if (Model.Title != FormData.Title)
            {
                AddToHistoryList(Histories,nameof(Model.Title), Model.Title, FormData.Title);
            }

            if (Model.Description != FormData.Description)
            {
                AddToHistoryList(Histories,nameof(Model.Description), Model.Description, FormData.Description);
            }

            if (Model.TicketPriorityId != FormData.TicketPriorityId)
            {
                AddToHistoryList(Histories,
                    nameof(Model.TicketPriorityId).GetPropertyName(),
                    Model.TicketPriorityId.ToString(),
                    FormData.TicketPriorityId.ToString());
            }

            if (Model.TicketStatusId != FormData.TicketStatusId)
            {
                AddToHistoryList(Histories,
                      nameof(Model.TicketStatusId).GetPropertyName(),
                      Model.TicketStatusId.ToString(),
                      FormData.TicketStatusId.ToString());
            }

            if (Model.TicketTypeId != FormData.TicketTypeId)
            {
                AddToHistoryList(Histories,
                     nameof(Model.TicketTypeId).GetPropertyName(),
                     Model.TicketTypeId.ToString(),
                     FormData.TicketTypeId.ToString());

            }

            return Histories;
        }

        private void AddToHistoryList(List<History> histories, string property, string oldValue, string newValue)
        {
            var history = new History();
            history.TicketId = Model.Id;
            history.Property = property;
            history.OldValueName = oldValue;
            history.NewValueName = newValue;

            if (property == "Status")
            {
                history.OldValueName = Db.TicketStatuses.FirstOrDefault(p => p.Id.ToString() == oldValue).Name;
                history.NewValueName = Db.TicketStatuses.FirstOrDefault(p => p.Id.ToString() == newValue).Name;
            }
            else if (property == "Priority")
            {
                history.OldValueName = Db.TicketPriorites.FirstOrDefault(p => p.Id.ToString() == oldValue).Name;
                history.NewValueName = Db.TicketPriorites.FirstOrDefault(p => p.Id.ToString() == newValue).Name;
            }
            else if (property == "Type")
            {
                history.OldValueName = Db.TicketTypes.FirstOrDefault(p => p.Id.ToString() == oldValue).Name;
                history.NewValueName = Db.TicketTypes.FirstOrDefault(p => p.Id.ToString() == newValue).Name;
            }
            histories.Add(history);
        }

    }
}