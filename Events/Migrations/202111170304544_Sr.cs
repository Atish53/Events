namespace Events.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Sr : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.EventBookings", "CustomerSurname");
            DropColumn("dbo.EventBookings", "IdNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EventBookings", "IdNumber", c => c.String());
            AddColumn("dbo.EventBookings", "CustomerSurname", c => c.String());
        }
    }
}
