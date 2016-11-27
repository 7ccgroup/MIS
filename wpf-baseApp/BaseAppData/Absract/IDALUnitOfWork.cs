using BaseAppData.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseAppData.Absract
{
    public interface IDALUnitOfWork : IUnitOfWork
    {
        IRepository<POS_Setup> POS_Setups { get; }

    }
}
