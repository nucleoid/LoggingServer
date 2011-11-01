
using System;
using System.Collections.Generic;

namespace LoggingServer.Server.Domain
{
    public class Subscription
    {
        public Subscription()
        {
            EmailList = string.Empty;
        }

        public virtual Guid ID { get; set; }
        public virtual SearchFilter Filter { get; set; }
        public virtual bool IsDailyOverview { get; set; }
        public virtual string EmailList { get; set; }

        public virtual IList<string> Emails 
        { 
            get { return EmailList.Split(','); } 
            set { EmailList = string.Join(",", value); } 
        }
    }
}
