using System;
using System.Data.SqlClient;
using System.Reflection;
using FluentMigrator;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Generators.SqlServer;
using FluentMigrator.Runner.Initialization;
using FluentMigrator.Runner.Processors;
using FluentMigrator.Runner.Processors.SqlServer;

namespace LoggingServer.Server.Repository.FluentMigrations
{
    public class Runner
    {
        private readonly string _connectionString;
        private readonly Assembly _assembly;

        /// <summary>
        /// Migration runner
        /// </summary>
        /// <param name="connectionString">Database connection string</param>
        /// <param name="assembly">Assembly containing the migration classes to run</param>
        public Runner(string connectionString, Assembly assembly)
        {
            _connectionString = connectionString;
            _assembly = assembly;
            Announcer = new NullAnnouncer();
            Options = new ProcessorOptions();
            MigrationGenerator = new SqlServer2005Generator();
        }

        /// <summary>
        /// Defaults to FluentMigrator.Runner.Generators.SqlServer.SqlServer2008Generator
        /// </summary>
        public virtual IMigrationGenerator MigrationGenerator { get; set; }

        /// <summary>
        /// Defaults to FluentMigrator.Runner.Announcers.NullAnnouncer
        /// </summary>
        public virtual IAnnouncer Announcer { get; set; }

        /// <summary>
        /// Defaults to emtpy FluentMigrator.Runner.Processors.ProcessorOptions
        /// </summary>
        public virtual IMigrationProcessorOptions Options { get; set; }

        /// <summary>
        /// Runs the migration
        /// </summary>
        public virtual void Run()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var context = new RunnerContext(Announcer);
                var processor = GenerateProcessor(connection);
                var runner = GenerateRunner(context, processor);
                runner.MigrateUp();
            }
        }

        internal virtual IMigrationProcessor GenerateProcessor(SqlConnection connection)
        {
            return new SqlServerProcessor(connection, MigrationGenerator, Announcer, Options);
        }

        internal virtual IMigrationRunner GenerateRunner(IRunnerContext context, IMigrationProcessor processor)
        {
            return new MigrationRunner(_assembly, context, processor);
        }
    }
}
