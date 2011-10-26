using AutoMapper;
using LoggingServer.Interface.Models;
using LoggingServer.Server.Domain;

namespace LoggingServer.Interface.Automapper
{
    public static class AutomapperConfig
    {
        public static void Setup()
        {
            Mapper.Reset();

            Mapper.CreateMap<LogEntry, LogEntryModel>()
                .ForMember(d => d.Project, o => o.MapFrom(s => s.Component != null ? s.Component.Project != null ? s.Component.Project.Name : "N/A" : "N/A"))
                .ForMember(d => d.Component, o => o.MapFrom(s => s.Component != null ? s.Component.Name : "N/A"))
                .ForMember(d => d.ComponentID, o => o.MapFrom(s => s.EntryAssemblyGuid));

            Mapper.CreateMap<LogEntry, LogEntrySummaryModel>()
                .ForMember(d => d.Project, o => o.MapFrom(s => s.Component != null ? s.Component.Project != null ? s.Component.Project.Name : "N/A" : "N/A"))
                .ForMember(d => d.Component, o => o.MapFrom(s => s.Component != null ? s.Component.Name : "N/A"));
        }
    }
}