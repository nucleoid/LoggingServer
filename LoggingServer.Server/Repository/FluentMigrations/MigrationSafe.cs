using System;
using FluentMigrator;

namespace LoggingServer.Server.Repository.FluentMigrations
{
    public abstract class MigrationSafe : Migration
    {
        public override sealed void Down()
        {
            throw new NotSupportedException("Backward migration not supported");
        }
    }
}
