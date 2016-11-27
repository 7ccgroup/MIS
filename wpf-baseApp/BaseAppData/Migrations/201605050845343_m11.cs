namespace BaseAppData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.POS_OrderHeader", "vApprovalCode", c => c.String(maxLength: 100));
            AddColumn("dbo.POS_OrderHeader", "TxnId", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.POS_OrderHeader", "TxnId");
            DropColumn("dbo.POS_OrderHeader", "vApprovalCode");
        }
    }
}
