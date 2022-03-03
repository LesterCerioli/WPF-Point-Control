using ControlePonto.Domain.services.persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Tests.mocks
{
    public class UnitOfWorkMock : IUnitOfWork
    {
        public NHibernate.ISession Session
        {
            get { return null; }
        }

        public void openConnection()
        {
            
        }

        public void Dispose()
        {
            
        }
    }
}
