
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LoggingServer.Server.Domain;
using LoggingServer.Server.Repository;
using NLog.LogReceiverService;

namespace LoggingServer.Server
{
    public class LogReceiverServer : ILogReceiverServer
    {
        private readonly static List<PropertyInfo> PropertyInfos = (typeof(LogEntry)).GetProperties().ToList();
        private readonly IWritableRepository<Component> _componentRepository;
        private readonly IWritableRepository<LogEntry> _logEntryRepository;

        public LogReceiverServer(IWritableRepository<LogEntry> logEntryRepository, IWritableRepository<Component> componentRepository)
        {
            _logEntryRepository = logEntryRepository;
            _componentRepository = componentRepository;
        }

        public void ProcessLogMessages(NLogEvents events)
        {
            var data = new List<LogEntry>();

            var eventInfos = events.ToEventInfo();

            foreach (var ev in eventInfos)
            {
                var log = new LogEntry { ClientID = events.ClientName };

                foreach (var pi in PropertyInfos)
                {
                    var value = ev.Properties.FirstOrDefault(x => (string)x.Key == pi.Name).Value;

                    if (value == null)
                        continue;

                    if (pi.PropertyType == typeof(LogLevel))
                    {
                        SetLogLevel(log, value, pi);
                    }
                    else if (pi.PropertyType == typeof(int) || pi.PropertyType == typeof(bool))
                    {
                        pi.SetValue(log, Convert.ChangeType(value, pi.PropertyType), null);
                    }
                    else if (pi.PropertyType == typeof(Guid))
                    {
                        if (pi.Name == "EntryAssemblyGuid")
                        {
                            if (ev.Properties.ContainsKey("EntryAssemblyTitle") && ev.Properties.ContainsKey("EntryAssemblyDescription"))
                            {
                                var title = ev.Properties.FirstOrDefault(x => (string)x.Key == "EntryAssemblyTitle").Value.ToString();
                                var description = ev.Properties.FirstOrDefault(x => (string)x.Key == "EntryAssemblyDescription").Value.ToString();
                                SetComponent(log, value, title, description);
                            }
                        }
                        pi.SetValue(log, new Guid(value.ToString()), null);
                    }
                    else if (pi.PropertyType == typeof(DateTime))
                    {
                        pi.SetValue(log, DateTime.Parse(value.ToString()), null);
                    }
                    else if (pi.PropertyType == typeof(TimeSpan))
                    {
                        pi.SetValue(log, TimeSpan.Parse(value.ToString()), null);
                    }
                    else
                    {
                        pi.SetValue(log, value.ToString(), null);
                    }
                }

                data.Add(log);
            }

            _logEntryRepository.Save(data);
        }

        private static void SetLogLevel(LogEntry log, object value, PropertyInfo pi)
        {
            LogLevel level;

            try
            {
                level = (LogLevel) Enum.Parse(typeof (LogLevel), value.ToString());
            }
            catch
            {
                level = LogLevel.Off;
            }

            pi.SetValue(log, level, null);
        }

        private void SetComponent(LogEntry log, object value, string title, string description)
        {
            var component = _componentRepository.Get(new Guid(value.ToString()));
            if(component == null)
            {
                component = new Component {ID = new Guid(value.ToString()), Name = title, Description = description, DateAdded = DateTime.Now};
                _componentRepository.Save(component);
            } 
            else if(component.Name != title || component.Description != description)
            {
                component.Name = title;
                component.Description = description;
                _componentRepository.Save(component);
            }
            
            log.Component = component;
        }
    }
}
