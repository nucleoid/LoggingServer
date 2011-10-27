using System;
using System.ComponentModel.DataAnnotations;
using LoggingServer.Server.Domain;

namespace LoggingServer.Interface.Models
{
    public class LogEntrySummaryModel
    {
        [ScaffoldColumn(false)]
        public Guid ID { get; set; }
        public string Project { get; set; }
        public string Component { get; set; }
        public LogLevel LogLevel { get; set; }
        public string LogMessage { get; set; }
        public string MachineName { get; set; }
        public DateTime DateAdded { get; set; }
    }
}