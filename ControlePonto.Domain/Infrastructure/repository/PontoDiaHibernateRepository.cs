using ControlePonto.Domain.ponto;
using ControlePonto.Domain.ponto.trabalho;
using ControlePonto.Domain.services.persistence;
using ControlePonto.Domain.usuario;
using ControlePonto.Domain.usuario.funcionario;
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
    public class PontoDiaHibernateRepository : IPontoDiaRepository
    {
        private IUnitOfWork UnitOfWork;
        private ISession Session { get { return UnitOfWork.Session; } }
        public PontoDiaHibernateRepository(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public ulong save(PontoDia ponto)
        {
            using (ITransaction trx = Session.BeginTransaction())
            {
                Session.SaveOrUpdate(ponto);
                trx.Commit();
                return ponto.Id;
            }            
        }

        public List<DiaTrabalho> findPontosAbertos(Funcionario funcionario)
        {
            return Session.CreateCriteria<DiaTrabalho>()
                .Add(Restrictions.IsNull("Fim"))
                .Add(Restrictions.Eq("Funcionario", funcionario))
                .List<DiaTrabalho>().ToList();
        }

        public bool existePontoDia(Funcionario funcionario, DateTime date)
        {
            return
            Session.CreateCriteria<PontoDia>()
                .Add(Restrictions.Eq("Data", date.Date))
                .Add(Restrictions.Eq("Funcionario", funcionario))
                .SetProjection(Projections.RowCount())
                .UniqueResult<int>() > 0;
        }


        public DiaTrabalho findPontoAberto(Funcionario funcionario, DateTime date)
        {
            return Session.CreateCriteria<DiaTrabalho>()
                .Add(Restrictions.IsNull("Fim"))
                .Add(Restrictions.Eq("Funcionario", funcionario))
                .Add(Restrictions.Eq("Data", date.Date))
                .SetFetchMode("Intervalos", FetchMode.Eager)
                .SetFetchMode("Intervalos.TipoIntervalo", FetchMode.Eager)
                .UniqueResult<DiaTrabalho>();
        }


        public List<PontoDia> findPontosNoIntervalo(Funcionario funcionario, DateTime inicio, DateTime fim, bool lazyLoadTrabalho = true, bool lazyLoadFolga = true)
        {
            FetchMode fetchDiaTrabalho = lazyLoadTrabalho ? FetchMode.Lazy : FetchMode.Eager;
            FetchMode fetchDiaFolga = lazyLoadFolga ? FetchMode.Lazy : FetchMode.Eager;

            return
                Session
                    .CreateCriteria<PontoDia>()
                    .SetFetchMode("DiaTrabalho", fetchDiaTrabalho)                        
                    .SetFetchMode("Intervalos", fetchDiaTrabalho)
                    .SetFetchMode("Feriado", fetchDiaTrabalho)
                    .SetFetchMode("DiaFolga", fetchDiaFolga)
                    .Add(Restrictions.Eq("Funcionario", funcionario))
                    .Add(Restrictions.Between("Data", inicio, fim))
                    .List<PontoDia>().ToList();            
        }


        public DiaTrabalho findPontoTrabalho(Funcionario funcionario, DateTime date)
        {
            return Session.CreateCriteria<DiaTrabalho>()
                .Add(Restrictions.Eq("Funcionario", funcionario))
                .Add(Restrictions.Eq("Data", date))
                .SetFetchMode("Funcionario", FetchMode.Eager)
                .SetFetchMode("Intervalos", FetchMode.Eager)
                .SetFetchMode("Intervalos.TipoIntervalo", FetchMode.Eager)
                .UniqueResult<DiaTrabalho>();
        }
    }
}
