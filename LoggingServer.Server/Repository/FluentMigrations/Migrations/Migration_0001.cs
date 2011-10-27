using FluentMigrator;

namespace LoggingServer.Server.Repository.FluentMigrations.Migrations
{
    /// <summary>
    /// Created using the Migration Template located at the root of this project solution.
    /// In order to use it you will need to import it into your ReSharper File Templates.
    /// The template takes care of boiler plate code and the migration attribute number.
    /// </summary>
    [Migration(20111024032635)]
    public class Migration_0001 : MigrationSafe
    {
        public override void Up()
        {
            CreateProjectsTable();
            CreateComponentsTable();
            CreateLogEntriesTable();
            CreateSearchFiltersTable();
        }

        private void CreateProjectsTable()
        {
            Create.Table("Projects")
                .WithColumn("ID").AsGuid().PrimaryKey()
                .WithColumn("Version").AsInt32().NotNullable()
                .WithColumn("Name").AsString(255).Unique().Nullable()
                .WithColumn("Description").AsString(255).Nullable()
                .WithColumn("DateAdded").AsDateTime().Nullable();
        }

        private void CreateComponentsTable()
        {
            Create.Table("Components")
                .WithColumn("ID").AsGuid().PrimaryKey()
                .WithColumn("Version").AsInt32().NotNullable()
                .WithColumn("Name").AsString(255).Unique().Nullable()
                .WithColumn("Description").AsString(255).Nullable()
                .WithColumn("DateAdded").AsDateTime().Nullable()
                .WithColumn("ProjectID").AsGuid().Nullable();
            Create.ForeignKey("fk_Components_ProjectID_Projects_ID").FromTable("Components")
                .ForeignColumn("ProjectID").ToTable("Projects").PrimaryColumn("ID");
        }

        private void CreateLogEntriesTable()
        {
            Create.Table("LogEntries")
                .WithColumn("ID").AsGuid().PrimaryKey()
                .WithColumn("Version").AsInt32().NotNullable()
                .WithColumn("ExceptionStackTrace").AsString(int.MaxValue).Nullable()
                .WithColumn("ExceptionString").AsString(int.MaxValue).Nullable()
                .WithColumn("StackTrace").AsString(int.MaxValue).Nullable()
                .WithColumn("ExceptionMessage").AsString(int.MaxValue).Nullable()
                .WithColumn("ExceptionMethod").AsString(int.MaxValue).Nullable()
                .WithColumn("ExceptionType").AsString(int.MaxValue).Nullable()
                .WithColumn("BaseDirectory").AsString(int.MaxValue).Nullable()
                .WithColumn("CallSite").AsString(int.MaxValue).Nullable()
                .WithColumn("Counter").AsInt32().Nullable()
                .WithColumn("LogID").AsGuid().Nullable()
                .WithColumn("LogIdentity").AsString(255).Nullable()
                .WithColumn("LogLevel").AsString(255).Nullable()
                .WithColumn("Logger").AsString(255).Nullable()
                .WithColumn("LongDate").AsDateTime().Nullable()
                .WithColumn("MachineName").AsString(255).Nullable()
                .WithColumn("LogMessage").AsString(255).Nullable()
                .WithColumn("ProcessID").AsString(255).Nullable()
                .WithColumn("ProcessInfo").AsString(255).Nullable()
                .WithColumn("ProcessName").AsString(255).Nullable()
                .WithColumn("ProcessTime").AsInt64().Nullable()
                .WithColumn("ThreadID").AsString(255).Nullable()
                .WithColumn("ThreadName").AsString(255).Nullable()
                .WithColumn("WindowsIdentity").AsString(255).Nullable()
                .WithColumn("EntryAssemblyCompany").AsString(255).Nullable()
                .WithColumn("EntryAssemblyDescription").AsString(255).Nullable()
                .WithColumn("EntryAssemblyGuid").AsGuid().Nullable()
                .WithColumn("EntryAssemblyProduct").AsString(255).Nullable()
                .WithColumn("EntryAssemblyTitle").AsString(255).Nullable()
                .WithColumn("EntryAssemblyVersion").AsString(255).Nullable()
                .WithColumn("EnvironmentKey").AsString(255).Nullable()
                .WithColumn("DateAdded").AsDateTime().Nullable()
                .WithColumn("NotificationsQueued").AsBoolean().Nullable();
        }

        private void CreateSearchFiltersTable()
        {
            Create.Table("SearchFilters")
                .WithColumn("ID").AsGuid().PrimaryKey()
                .WithColumn("UserName").AsString(255).Nullable()
                .WithColumn("ProjectName").AsString(255).Nullable()
                .WithColumn("ComponentName").AsString(255).Nullable()
                .WithColumn("StartDate").AsDateTime().Nullable()
                .WithColumn("EndDate").AsDateTime().Nullable()
                .WithColumn("LogLevel").AsString(255).Nullable()
                .WithColumn("MachineNamePartial").AsString(255).Nullable()
                .WithColumn("ExceptionPartial").AsString(255).Nullable()
                .WithColumn("MessagePartial").AsString(255).Nullable()
                .WithColumn("IsGlobal").AsBoolean().NotNullable();
        }
    }
}
