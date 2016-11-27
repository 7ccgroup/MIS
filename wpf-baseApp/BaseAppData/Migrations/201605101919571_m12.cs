namespace BaseAppData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m12 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.POS_OrderHeader", "dOrderTip", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.POS_OrderHeader", "dOrderTip");
        }
    }
}
