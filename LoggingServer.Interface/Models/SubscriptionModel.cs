
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LoggingServer.Server.Domain;

namespace LoggingServer.Interface.Models
{
    public class SubscriptionModel : IFilterModel
    {
        [ScaffoldColumn(false)]
        public Guid ID { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        public IList<string> Emails { get; set; }

        [Display(Name = "Email addresses")]
        public string EmailList { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        public Guid FilterId { get; set; }

        [Display(Name = "Saved Search")]
        public string FilterDescription { get; set; }

        public bool IsDailyOverview { get; set; }
    }
}