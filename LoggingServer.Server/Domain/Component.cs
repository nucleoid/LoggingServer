
using System;
using System.Collections.Generic;

namespace LoggingServer.Server.Domain
{
    public class Component
    {
        public virtual Guid ID { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual DateTime DateAdded { get; set; }
        public virtual int Version { get; set; }

        public virtual Project Project { get; set; }

        public virtual IList<LogEntry> LogEntries { get; set; }

        public virtual bool Equals(Component other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.ID.Equals(ID);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Component)) return false;
            return Equals((Component) obj);
        }
    }
}
