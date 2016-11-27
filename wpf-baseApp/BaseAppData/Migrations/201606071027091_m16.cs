namespace BaseAppData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m16 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.POS_ItemMaster", "vItemmodifier", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.POS_ItemMaster", "vItemmodifier", c => c.String(maxLength: 4000));
        }
    }
}
