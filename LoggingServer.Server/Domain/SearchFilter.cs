using System;
using System.Collections.Generic;
using System.Linq;

namespace LoggingServer.Server.Domain
{
    public class SearchFilter
    {
        public virtual Guid ID { get; set; }
        public virtual string UserName { get; set; }
        public virtual string ProjectName { get; set; }
        public virtual string ComponentName { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual LogLevel? LogLevel { get; set; }
        public virtual string MachineNamePartial { get; set; }
        public virtual string ExceptionPartial { get; set; }
        public virtual string MessagePartial { get; set; }
        public virtual bool IsGlobal { get; set; }

        public virtual string Description
        {
            get
            {
                var descriptions = new List<string>();
                if(!string.IsNullOrEmpty(ProjectName))
                    descriptions.Add(string.Format("Project: {0}", ProjectName));
                if (!string.IsNullOrEmpty(ComponentName))
                    descriptions.Add(string.Format("Component: {0}", ComponentName));
                if (!string.IsNullOrEmpty(UserName))
                    descriptions.Add(string.Format("User: {0}", UserName));
                if (LogLevel.HasValue)
                    descriptions.Add(string.Format("Log Level: {0}", LogLevel));
                if (StartDate.HasValue)
                    descriptions.Add(string.Format("Start: {0}", StartDate));
                if (EndDate.HasValue)
                    descriptions.Add(string.Format("End: {0}", EndDate));
                if (!string.IsNullOrEmpty(ExceptionPartial))
                    descriptions.Add(string.Format("Exception: {0}", ExceptionPartial));
                if (!string.IsNullOrEmpty(MessagePartial))
                    descriptions.Add(string.Format("Message: {0}", MessagePartial));
                if (!string.IsNullOrEmpty(MachineNamePartial))
                    descriptions.Add(string.Format("Machine Name: {0}", MachineNamePartial));
                descriptions.Add(string.Format("Global: {0}", IsGlobal));

                if (descriptions.Count == 1)
                    return string.Format("All - {0}", descriptions.SingleOrDefault());
                return string.Join(" - ", descriptions);
            }
        }
    }
}
