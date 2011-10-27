using System;
using System.ComponentModel.DataAnnotations;
using LoggingServer.Server.Domain;

namespace LoggingServer.Interface.Models
{
    public class SearchFilterModel
    {
        [ScaffoldColumn(false)]
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
    }
}