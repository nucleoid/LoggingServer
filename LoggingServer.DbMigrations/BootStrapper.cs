
using System;
using System.Configuration;
using FluentMigrator.Runner;
using LoggingServer.DbMigrations.FluentMigrations;

namespace LoggingServer.DbMigrations
{
    public static class BootStrapper
    {
        private static Runner _runner;

        public static void Start(IAnnouncer announcer)
        {
            var _runner = new Runner(ConfigurationManager.ConnectionStrings["Default"].ConnectionString, typeof(Runner).Assembly);
            _runner.Announcer = announcer;
            _runner.Run();
        }

        public static MigrationState GetDatabaseMigrationState()
        {
            if (_runner == null)
                throw new Exception("Start needs to be called before this method");
            return _runner.GetMigrationState();
        }
    }
}
