using ControlePonto.Domain.feriado;
using ControlePonto.Infrastructure.nhibernate;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Infrastructure.repository
{
    public class FeriadoHibernateRepository : IFeriadoRepository
    {
        public uint save(Feriado feriado)
        {
            using (ISession session = NHibernateHelper.openSession())
            using (ITransaction trx = session.BeginTransaction())
            {
                session.SaveOrUpdate(feriado);
                trx.Commit();
                return feriado.Id;
            }
        }

        public List<Feriado> findFromMonth(int mes)
        {
            using (ISession session = NHibernateHelper.openSession())
            {
                return
                session.CreateCriteria<Feriado>()
                    .Add(Restrictions.Eq("Mes", mes))
                    .List<Feriado>().ToList();
            }
        }
    }
}
