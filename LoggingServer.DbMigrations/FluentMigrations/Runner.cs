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

namespace LoggingServer.DbMigrations.FluentMigrations
{
    public sealed class Runner
    {
        private readonly string _connectionString;
        private readonly Assembly _assembly;
        private IMigrationLoader _migrationLoader;
        private IVersionLoader _versionLoader;

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
        public IMigrationGenerator MigrationGenerator { get; set; }

        /// <summary>
        /// Defaults to FluentMigrator.Runner.Announcers.NullAnnouncer
        /// </summary>
        public IAnnouncer Announcer { get; set; }

        /// <summary>
        /// Defaults to emtpy FluentMigrator.Runner.Processors.ProcessorOptions
        /// </summary>
        public IMigrationProcessorOptions Options { get; set; }

        /// <summary>
        /// Runs the migration
        /// </summary>
        public void Run()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var context = new RunnerContext(Announcer);
                var processor = GenerateProcessor(connection);
                var runner = GenerateRunner(context, processor);
                _migrationLoader = runner.MigrationLoader;
                _versionLoader = runner.VersionLoader;
                runner.MigrateUp();
            }
        }

        public MigrationState GetMigrationState()
        {
            var state = new MigrationState();
            try
            {
                state.Total = _migrationLoader.Migrations.Keys.Count;

                foreach (var mig in _migrationLoader.Migrations.Keys)
                {
                    if (_versionLoader.VersionInfo.HasAppliedMigration(mig))
                        state.Completed++;
                    else
                        state.Pending++;
                }
            }
            catch (Exception ex)
            {
                state.Exception = ex;
            }

            return state;
        }

        internal IMigrationProcessor GenerateProcessor(SqlConnection connection)
        {
            return new SqlServerProcessor(connection, MigrationGenerator, Announcer, Options);
        }

        internal MigrationRunner GenerateRunner(IRunnerContext context, IMigrationProcessor processor)
        {
            return new MigrationRunner(_assembly, context, processor);
        }
    }
}
