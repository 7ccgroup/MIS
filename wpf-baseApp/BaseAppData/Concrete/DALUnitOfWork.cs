using BaseAppData.Absract;
using BaseAppData.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BaseAppData.Concrete
{
    public class DALUnitOfWork : IDALUnitOfWork
    {
        private EFDbContext dbContext;
        private IRepository<POS_Setup> _pos_setups;

        public IRepository<POS_Setup> POS_Setups
        {
            get
            {

                if (_pos_setups == null)
                    _pos_setups = new Repository<POS_Setup>(dbContext);

                return _pos_setups;
            }

        }

    

        public DALUnitOfWork()
        {
            dbContext = new EFDbContext();
        }



       

        public int SaveChanges()
        {
            return dbContext.SaveChanges();
            
        }

        public void StateChange(object entity,EntityState state)
        {
            dbContext.Entry(entity).State = state;

        }



        public void Rollback(object entity)
        {

            DbEntityEntry entry = dbContext.Entry(entity);

            if (entry != null)
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Deleted:
                        entry.Reload();
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    default: break;
                }
            } 


         

          
        }

        public void Dispose()
        {

           
            if (dbContext != null)
                dbContext.Dispose();
            GC.SuppressFinalize(this);
        }

      
    }
}
