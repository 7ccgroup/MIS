using BaseAppData.Entity;
using BaseAppData.Migrations;
using System.Data.Entity;

namespace BaseAppData.Concrete
{
    public class EFDbContext : DbContext
    {

        public DbSet<POS_Setup> POS_Setups { get; set; }
        public DbSet<POS_ItemCategory> POS_ItemCategories { get; set; }
        public DbSet<POS_ItemMaster> POS_ItemMasters { get; set; }
        public DbSet<POS_Customer> POS_Customers { get; set; }
        public DbSet<POS_OrderHeader> POS_OrderHeaders { get; set; }
        public DbSet<POS_OrderDetails> POS_OrderDetails { get; set; }
        public DbSet<POS_CodesTable> POS_CodesTable { get; set; } //added to create a table POS_CodesTable Then add class in Entity Folder
        public DbSet<POS_User> POS_User { get; set; } //added to create user id in database
        public DbSet<POS_TimeSheet> POS_TimeSheet { get; set; } //added to create user time Sheet in database


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //changes to integrate db....
            if (BaseAppData.Properties.Settings.Default.MigrateDatabaseChanges == "Yes")
            {
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<BaseAppData.Concrete.EFDbContext, Configuration>());

            }
            else
            {
                base.OnModelCreating(modelBuilder);
            }
        }
      

    }
}
