﻿using System;

namespace LoggingServer.Server.Domain
{
    public class SearchFilter
    {
        public virtual Guid ID { get; set; }
        public virtual string ProjectName { get; set; }
        public virtual string ComponentName { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual LogLevel? LogLevel { get; set; }
        public virtual string MachineNamePartial { get; set; }
        public virtual string ExceptionPartial { get; set; }
        public virtual string MessagePartial { get; set; }
    }
}
