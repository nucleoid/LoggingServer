using System;
using System.Collections.Generic;

namespace LoggingServer.Server.Domain
{
    public class Project
    {
        public virtual Guid ID { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual DateTime DateAdded { get; set; }
        public virtual int Version { get; set; }

        public virtual IList<Component> Components { get; set; }
    }
}
