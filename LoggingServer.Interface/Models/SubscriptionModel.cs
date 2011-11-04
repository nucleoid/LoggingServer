
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using LoggingServer.Server.Domain;
using Microsoft.Web.Mvc;

namespace LoggingServer.Interface.Models
{
    public class SubscriptionModel : IFilterModel
    {
        public SubscriptionModel()
        {
            EmailEntries = new List<EmailEntry>();
        }

        [ScaffoldColumn(false)]
        public Guid ID { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Email addresses")]
        public IList<EmailEntry> EmailEntries { get; set; }

        [ScaffoldColumn(false)]
        [Required]
        public IList<string> Emails
        {
            get { return EmailEntries.Select(x => x.Email).ToList(); }
            set 
            {
                foreach (var email in value)
                {
                    EmailEntries.Add(new EmailEntry {Email = email});
                }
            }
        }

        [Display(Name = "Email addresses")]
        public string EmailList { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        [Display(Name = "Saved Search")]
        public Guid FilterId { get; set; }

        [Display(Name = "Saved Search")]
        public string FilterDescription { get; set; }

        public bool IsDailyOverview { get; set; }
    }

    public class EmailEntry
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}