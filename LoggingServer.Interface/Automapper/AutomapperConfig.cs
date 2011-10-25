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
                .ForMember(d => d.ComponentProjectName, o => o.NullSubstitute("N/A"))
                .ForMember(d => d.ComponentID, o => o.MapFrom(s => s.EntryAssemblyGuid))
                .ForMember(d => d.ComponentName, o => o.NullSubstitute("N/A"));
        }
    }
}