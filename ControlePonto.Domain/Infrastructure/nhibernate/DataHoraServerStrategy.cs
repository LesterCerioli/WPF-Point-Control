using ControlePonto.Domain.ponto;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Infrastructure.nhibernate
{
    public class DataHoraServerStrategy : IDataHoraStrategy
    {
        public DateTime getDataHoraAtual()
        {
            using (ISession session = NHibernateHelper.openSession())
            {
                var sql = "SELECT NOW()";
                var query = session.CreateSQLQuery(sql);
                var result = query.UniqueResult();

                return Convert.ToDateTime(result);
            }
        }
    }
}
