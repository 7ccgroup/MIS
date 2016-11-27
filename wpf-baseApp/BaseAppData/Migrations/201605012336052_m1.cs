namespace BaseAppData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.POS_CodesTable",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        vTableCode = c.String(maxLength: 16),
                        vFldCode = c.String(maxLength: 16),
                        vDescription = c.String(maxLength: 255),
                        vDecode = c.String(maxLength: 255),
                        vRelatedDecode = c.String(maxLength: 255),
                        vSeqNum = c.String(maxLength: 5),
                        vUsage = c.String(maxLength: 16),
                        vEntryBy = c.String(maxLength: 16),
                        tTimestamp = c.Long(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.POS_Customer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        iCustid = c.Int(nullable: false),
                        vCustName = c.String(maxLength: 50),
                        vCustContactNme = c.String(maxLength: 50),
                        vCustPrimaryPh = c.String(maxLength: 50),
                        vCustPhone2 = c.String(maxLength: 50),
                        vCustPhone3 = c.String(maxLength: 50),
                        vCustPhone4 = c.String(maxLength: 50),
                        vCustFax1 = c.String(maxLength: 50),
                        vCustFax2 = c.String(maxLength: 50),
                        vCustEmail = c.String(maxLength: 50),
                        vCustAddress1 = c.String(maxLength: 50),
                        vCustAddress2 = c.String(maxLength: 50),
                        vCustCity = c.String(maxLength: 50),
                        vCustState = c.String(maxLength: 50),
                        vCustCountry = c.String(maxLength: 50),
                        vCustZipCode = c.String(maxLength: 50),
                        vCustBillAddress1 = c.String(maxLength: 50),
                        vCustBillAddress2 = c.String(maxLength: 50),
                        vCustBillCity = c.String(maxLength: 50),
                        vCustBillCountry = c.String(maxLength: 50),
                        vCustBillZipCode = c.String(maxLength: 50),
                        vCustShipAddress1 = c.String(maxLength: 50),
                        vCustShipAddress2 = c.String(maxLength: 50),
                        vCustShipCity = c.String(maxLength: 50),
                        vCustShipState = c.String(maxLength: 50),
                        vCustShipCountry = c.String(maxLength: 50),
                        vCustShipZipCode = c.String(maxLength: 50),
                        vCustAccountNum = c.String(maxLength: 50),
                        vCustNote1 = c.String(maxLength: 255),
                        vCustNote2 = c.String(maxLength: 255),
                        vCustNote3 = c.String(maxLength: 255),
                        vCustCategory = c.String(maxLength: 8),
                        vCustComments = c.String(maxLength: 255),
                        vCustStatus = c.String(maxLength: 8),
                        vEntryBy = c.String(maxLength: 8),
                        tTimestamp = c.Long(),
                        POS_Setup_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.POS_Setup", t => t.POS_Setup_Id)
                .Index(t => t.POS_Setup_Id);
            
            CreateTable(
                "dbo.POS_OrderHeader",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        iOrderID = c.Int(nullable: false),
                        iRelatedOrderID = c.Int(),
                        vOrderNumber = c.Int(nullable: false),
                        vOrderRef = c.String(maxLength: 25),
                        vOrderType = c.String(maxLength: 10),
                        vOrderMove = c.String(maxLength: 10),
                        vOrderAcct = c.String(maxLength: 75),
                        vOrderSubAcct = c.String(maxLength: 75),
                        dOrderOrigAmtDue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        dOrderAmtDue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        dOrderAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        dOrderTax = c.Decimal(nullable: false, precision: 18, scale: 2),
                        vOrderCurr = c.String(maxLength: 10),
                        vOrderNotes = c.String(maxLength: 200),
                        vOrderStatus = c.String(maxLength: 100),
                        vOrderPaymentType = c.String(maxLength: 50),
                        vOrderPaymentRef = c.String(maxLength: 25),
                        vOrderEmail = c.String(maxLength: 100),
                        vOrderComments1 = c.String(maxLength: 100),
                        vOrderComments2 = c.String(maxLength: 100),
                        vOrderComments3 = c.String(maxLength: 100),
                        vOrderComments4 = c.String(maxLength: 100),
                        vOrderComments5 = c.String(maxLength: 100),
                        vOrderComments6 = c.String(maxLength: 100),
                        vOrderComments7 = c.String(maxLength: 100),
                        vOrderComments8 = c.String(maxLength: 100),
                        dCreatedDate = c.DateTime(nullable: false),
                        dModifiedDate = c.DateTime(nullable: false),
                        vEntryBy = c.String(maxLength: 100),
                        tTimestamp = c.Long(),
                        CustomerId = c.Int(nullable: false),
                        POS_SetupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.POS_Customer", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.POS_Setup", t => t.POS_SetupId, cascadeDelete: true)
                .Index(t => t.vOrderNumber, unique: true, name: "ItemIndex")
                .Index(t => t.CustomerId)
                .Index(t => t.POS_SetupId);
            
            CreateTable(
                "dbo.POS_OrderDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        iOrderLineId = c.Int(nullable: false),
                        fOrderQty = c.Int(nullable: false),
                        fLineItemPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        vLineSplInstructions = c.String(maxLength: 50),
                        fLineSubTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        vComments = c.String(maxLength: 255),
                        vStatus = c.String(maxLength: 8),
                        dCreatedDate = c.DateTime(nullable: false),
                        dModifiedDate = c.DateTime(nullable: false),
                        vEntryBy = c.String(maxLength: 8),
                        tTimestamp = c.Long(),
                        POS_OrderHeaderId = c.Int(nullable: false),
                        POS_ItemMasterId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.POS_ItemMaster", t => t.POS_ItemMasterId, cascadeDelete: true)
                .ForeignKey("dbo.POS_OrderHeader", t => t.POS_OrderHeaderId, cascadeDelete: true)
                .Index(t => t.POS_OrderHeaderId)
                .Index(t => t.POS_ItemMasterId);
            
            CreateTable(
                "dbo.POS_ItemMaster",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        itemID = c.Int(nullable: false),
                        vItemSku = c.String(maxLength: 30),
                        vItemDesc1 = c.String(maxLength: 200),
                        vItemDesc2 = c.String(maxLength: 200),
                        vItemPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        vItemRelatedID = c.Int(nullable: false),
                        vItemMinPrice = c.Decimal(precision: 18, scale: 2),
                        vItemVendor = c.String(maxLength: 200),
                        vItemVendorContact = c.String(maxLength: 100),
                        vItemVendorPhone = c.String(maxLength: 20),
                        vItemNotes = c.String(maxLength: 300),
                        vItemStatus = c.String(maxLength: 30),
                        vItemAvailability = c.String(maxLength: 30),
                        dStartDate = c.DateTime(nullable: false),
                        dEndDate = c.DateTime(nullable: false),
                        vEntryBy = c.String(maxLength: 50),
                        tTimestamp = c.Long(nullable: false),
                        POS_ItemCategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.POS_ItemCategory", t => t.POS_ItemCategoryId, cascadeDelete: true)
                .Index(t => t.itemID, unique: true, name: "ItemIndex")
                .Index(t => t.POS_ItemCategoryId);
            
            CreateTable(
                "dbo.POS_ItemCategory",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CatID = c.Int(nullable: false),
                        vCategoryCode = c.String(maxLength: 50),
                        vCategoryDesc = c.String(maxLength: 200),
                        vCategoryShortDesc = c.String(maxLength: 200),
                        vComments = c.String(maxLength: 500),
                        vStatus = c.String(maxLength: 8),
                        dStartDt = c.DateTime(),
                        dEndDt = c.DateTime(),
                        vEntryBy = c.String(maxLength: 200),
                        tTimestamp = c.Long(),
                        POS_Setup_Id = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.POS_Setup", t => t.POS_Setup_Id)
                .Index(t => t.POS_Setup_Id);
            
            CreateTable(
                "dbo.POS_Setup",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        vCorpName = c.String(maxLength: 200),
                        vCorpPhone = c.String(maxLength: 50),
                        vCorpAddress1 = c.String(maxLength: 500),
                        vCorpAddress2 = c.String(maxLength: 500),
                        vCorpCity = c.String(maxLength: 100),
                        vCorpState = c.String(maxLength: 100),
                        vCorpCountry = c.String(maxLength: 100),
                        vSalesTax = c.Decimal(precision: 18, scale: 2),
                        vReceiptComments = c.String(maxLength: 500),
                        vCorpEmail = c.String(maxLength: 200),
                        vCorpContact = c.String(maxLength: 200),
                        vAlertComments = c.String(maxLength: 500),
                        vStatus = c.String(maxLength: 100),
                        dStartDt = c.DateTime(),
                        dEndDt = c.DateTime(),
                        vEntryBy = c.String(maxLength: 8),
                        tTimestamp = c.Long(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.POS_OrderDetails", "POS_OrderHeaderId", "dbo.POS_OrderHeader");
            DropForeignKey("dbo.POS_OrderDetails", "POS_ItemMasterId", "dbo.POS_ItemMaster");
            DropForeignKey("dbo.POS_OrderHeader", "POS_SetupId", "dbo.POS_Setup");
            DropForeignKey("dbo.POS_ItemCategory", "POS_Setup_Id", "dbo.POS_Setup");
            DropForeignKey("dbo.POS_Customer", "POS_Setup_Id", "dbo.POS_Setup");
            DropForeignKey("dbo.POS_ItemMaster", "POS_ItemCategoryId", "dbo.POS_ItemCategory");
            DropForeignKey("dbo.POS_OrderHeader", "CustomerId", "dbo.POS_Customer");
            DropIndex("dbo.POS_ItemCategory", new[] { "POS_Setup_Id" });
            DropIndex("dbo.POS_ItemMaster", new[] { "POS_ItemCategoryId" });
            DropIndex("dbo.POS_ItemMaster", "ItemIndex");
            DropIndex("dbo.POS_OrderDetails", new[] { "POS_ItemMasterId" });
            DropIndex("dbo.POS_OrderDetails", new[] { "POS_OrderHeaderId" });
            DropIndex("dbo.POS_OrderHeader", new[] { "POS_SetupId" });
            DropIndex("dbo.POS_OrderHeader", new[] { "CustomerId" });
            DropIndex("dbo.POS_OrderHeader", "ItemIndex");
            DropIndex("dbo.POS_Customer", new[] { "POS_Setup_Id" });
            DropTable("dbo.POS_Setup");
            DropTable("dbo.POS_ItemCategory");
            DropTable("dbo.POS_ItemMaster");
            DropTable("dbo.POS_OrderDetails");
            DropTable("dbo.POS_OrderHeader");
            DropTable("dbo.POS_Customer");
            DropTable("dbo.POS_CodesTable");
        }
    }
}
