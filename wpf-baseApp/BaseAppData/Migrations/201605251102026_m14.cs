namespace BaseAppData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m14 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.POS_OrderDetails", "vCustomComments", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.POS_OrderDetails", "vCustomComments");
        }
    }
}
