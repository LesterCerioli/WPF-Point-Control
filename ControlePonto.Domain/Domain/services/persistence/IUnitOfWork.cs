using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.services.persistence
{
    public interface IUnitOfWork : IDisposable
    {
        NHibernate.ISession Session { get; }
        void openConnection();
    }
}
