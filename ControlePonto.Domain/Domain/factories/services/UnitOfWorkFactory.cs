using ControlePonto.Domain.services.persistence;
using ControlePonto.Infrastructure.nhibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.factories.services
{
    public static class UnitOfWorkFactory
    {
        public static UnitOfWork criarUnitOfWork()
        {
            return new UnitOfWork(NHibernateHelper.getSessionFactory());
        }
    }
}
