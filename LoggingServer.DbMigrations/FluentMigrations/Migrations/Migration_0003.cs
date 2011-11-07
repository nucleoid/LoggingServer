using FluentMigrator;

namespace LoggingServer.DbMigrations.FluentMigrations.Migrations
{
    /// <summary>
    /// Created using the Migration Template located at the root of this project solution.
    /// In order to use it you will need to import it into your ReSharper File Templates.
    /// The template takes care of boiler plate code and the migration attribute number.
    /// </summary>
    [Migration(20111101043332)]
    public class Migration_0003 : MigrationSafe
    {
        public override void Up()
        {
            Create.Table("Subscriptions")
                .WithColumn("ID").AsGuid().PrimaryKey()
                .WithColumn("EmailList").AsString(int.MaxValue).Nullable()
                .WithColumn("FilterID").AsGuid().Nullable()
                .WithColumn("IsDailyOverview").AsBoolean().NotNullable();
            Create.ForeignKey("fk_Subscriptions_FilterID_SearchFilters_ID").FromTable("Subscriptions")
                .ForeignColumn("FilterID").ToTable("SearchFilters").PrimaryColumn("ID");

            Create.Index("LogEntries_DateAdded_Index").OnTable("LogEntries").OnColumn("DateAdded").Descending().WithOptions().NonClustered();
        }
    }
}
