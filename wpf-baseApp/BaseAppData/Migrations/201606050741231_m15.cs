namespace BaseAppData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m15 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.POS_TimeSheet",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        iUserId = c.Int(nullable: false),
                        vUserCode = c.String(maxLength: 10),
                        vUserStartTime = c.String(maxLength: 4000),
                        vUserEndTime = c.String(maxLength: 10),
                        vStatus = c.String(maxLength: 10),
                        dShiftStartDt = c.DateTime(nullable: false),
                        dShiftEndDt = c.DateTime(nullable: false),
                        vEntryBy = c.String(maxLength: 8),
                        tTimestamp = c.Long(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.POS_User",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        iUserId = c.Int(nullable: false),
                        vUserCode = c.String(maxLength: 10),
                        vUserAltCode = c.String(maxLength: 10),
                        vUserName = c.String(maxLength: 100),
                        vUserType = c.String(maxLength: 10),
                        vUserOLS = c.String(maxLength: 10),
                        vUserStartTime = c.String(maxLength: 10),
                        vUserWorkShift = c.String(maxLength: 10),
                        vStatus = c.String(maxLength: 10),
                        dStartDt = c.DateTime(nullable: false),
                        dEndDt = c.DateTime(nullable: false),
                        vEntryBy = c.String(maxLength: 8),
                        tTimestamp = c.Long(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.POS_OrderDetails", "fLineItemAddonTotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.POS_ItemMaster", "vItemmodifier", c => c.String(maxLength: 4000));
            AlterColumn("dbo.POS_OrderDetails", "vLineitemaddon", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.POS_OrderDetails", "vLineitemaddon", c => c.String(maxLength: 25));
            DropColumn("dbo.POS_ItemMaster", "vItemmodifier");
            DropColumn("dbo.POS_OrderDetails", "fLineItemAddonTotal");
            DropTable("dbo.POS_User");
            DropTable("dbo.POS_TimeSheet");
        }
    }
}
