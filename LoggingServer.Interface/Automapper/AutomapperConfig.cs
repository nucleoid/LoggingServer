using System;
using AutoMapper;
using LoggingServer.Interface.Models;
using LoggingServer.Server.Autofac;
using LoggingServer.Server.Automapper;
using LoggingServer.Server.Domain;
using LoggingServer.Server.Repository;

namespace LoggingServer.Interface.Automapper
{
    public static class AutomapperConfig
    {
        public static void Setup()
        {
            Mapper.Reset();

            Mapper.Initialize(map =>
            {
                map.ConstructServicesUsing(DependencyContainer.Resolve);

                map.CreateMap<LogEntry, LogEntryModel>()
                 .ForMember(d => d.Project, o => o.MapFrom(s => s.Component != null ? s.Component.Project != null ? s.Component.Project.Name : "N/A" : "N/A"))
                 .ForMember(d => d.Component, o => o.MapFrom(s => s.Component != null ? s.Component.Name : "N/A"))
                 .ForMember(d => d.ComponentID, o => o.MapFrom(s => s.EntryAssemblyGuid));

                map.CreateMap<LogEntry, LogEntrySummaryModel>()
                    .ForMember(d => d.Project, o => o.MapFrom(s => s.Component != null ? s.Component.Project != null ? s.Component.Project.Name : "N/A" : "N/A"))
                    .ForMember(d => d.Component, o => o.MapFrom(s => s.Component != null ? s.Component.Name : "N/A"));

                map.CreateMap<SearchFilter, SearchFilterModel>();
                map.CreateMap<SearchFilterModel, SearchFilter>();

                map.CreateMap<Subscription, SubscriptionModel>();
                map.CreateMap<SubscriptionModel, Subscription>().ForMember(dest => dest.Filter, o => o.ResolveUsing<FilterResolver>());
            });
        }
    }
}