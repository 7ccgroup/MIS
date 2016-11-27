namespace BaseAppData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m13 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.POS_OrderDetails", "vLineItemsize", c => c.String(maxLength: 25));
            AddColumn("dbo.POS_OrderDetails", "vLineItemspice", c => c.String(maxLength: 25));
            AddColumn("dbo.POS_OrderDetails", "vLineitemaddon", c => c.String(maxLength: 25));
            AddColumn("dbo.POS_OrderDetails", "vLineItemcooked", c => c.String(maxLength: 25));
        }
        
        public override void Down()
        {
            DropColumn("dbo.POS_OrderDetails", "vLineItemcooked");
            DropColumn("dbo.POS_OrderDetails", "vLineitemaddon");
            DropColumn("dbo.POS_OrderDetails", "vLineItemspice");
            DropColumn("dbo.POS_OrderDetails", "vLineItemsize");
        }
    }
}
