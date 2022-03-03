using ControlePonto.Domain.jornada;
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
    public class JornadaTrabalhoHibernateRepository : IJornadaTrabalhoRepository
    {
        public JornadaTrabalho findJornadaAtiva()
        {
            using (ISession session = NHibernateHelper.openSession())
            {
                return
                session.CreateCriteria<JornadaTrabalho>()
                    .AddOrder(Order.Desc("Id"))
                    .SetMaxResults(1)
                    .UniqueResult<JornadaTrabalho>();
            }
        }

        public bool existeAlgumaJornada()
        {
            using (ISession session = NHibernateHelper.openSession())
            {
                return
                session.CreateCriteria<JornadaTrabalho>()
                    .SetProjection(Projections.RowCount())
                    .UniqueResult<int>() > 0;
            }
        }

        public uint save(JornadaTrabalho jornada)
        {
            using (ISession session = NHibernateHelper.openSession())
            using (ITransaction trans = session.BeginTransaction())
            {
                session.SaveOrUpdate(jornada);                
                trans.Commit();
                return jornada.Id;
            }
        }
    }
}
