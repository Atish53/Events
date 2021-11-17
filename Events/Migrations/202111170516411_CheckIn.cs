namespace Events.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CheckIn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventBookings", "isCheckedIn", c => c.Boolean(nullable: false));
            AddColumn("dbo.EventBookings", "CheckInTime", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.EventBookings", "CheckInTime");
            DropColumn("dbo.EventBookings", "isCheckedIn");
        }
    }
}
