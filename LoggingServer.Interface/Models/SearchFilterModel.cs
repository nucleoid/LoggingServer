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

        [Display(Name = "Project Name")]
        public virtual string ProjectName { get; set; }

        [Display(Name = "Component Name")]
        public virtual string ComponentName { get; set; }

        [Display(Name = "Start Date")]
        public virtual DateTime? StartDate { get; set; }

        [Display(Name = "End Date")]
        public virtual DateTime? EndDate { get; set; }
        public virtual LogLevel? LogLevel { get; set; }

        [Display(Name = "Machine Name")]
        public virtual string MachineNamePartial { get; set; }

        [Display(Name = "Exception")]
        public virtual string ExceptionPartial { get; set; }

        [Display(Name = "Message")]
        public virtual string MessagePartial { get; set; }

        [Display(Name = "Global")]
        public virtual bool IsGlobal { get; set; }
    }
}