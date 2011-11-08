
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
            _runner = new Runner(ConfigurationManager.ConnectionStrings["Default"].ConnectionString, typeof(Runner).Assembly);
            _runner.Announcer = announcer;
            _runner.Run();
        }

        public static MigrationState GetDatabaseMigrationState()
        {
            if (_runner == null)
                return new MigrationState();
            return _runner.GetMigrationState();
        }
    }
}
