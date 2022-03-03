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
    public class UsuarioHibernateRepository : IUsuarioRepositorio
    {
        public Usuario findById(int id)
        {
            using (ISession session = NHibernateHelper.openSession())
                return session.Get<Usuario>(id);
        }

        public Usuario findByLogin(string login)
        {
            using (ISession session = NHibernateHelper.openSession())
            {
                return session
                    .CreateCriteria<Usuario>()
                    .Add(Restrictions.Eq("Login", login))
                    .UniqueResult<Usuario>();
            }
        }

        public int getCount()
        {
            using (ISession session = NHibernateHelper.openSession())
            {
                return session
                    .QueryOver<Usuario>()
                    .Select(Projections.RowCount())
                    .SingleOrDefault<int>();
            }
        }

        public uint save(Usuario usuario)
        {
            using (ISession session = NHibernateHelper.openSession())
            using (ITransaction trx = session.BeginTransaction())
            {
                session.SaveOrUpdate(usuario);
                trx.Commit();
                return usuario.Id;
            }
        }


        public bool loginExiste(string login)
        {
            using (ISession session = NHibernateHelper.openSession())
            {
                return session.QueryOver<Usuario>()
                    .Where(x => x.Login == login)
                    .RowCount() > 0;
            }
        }


        public List<Funcionario> findFuncionarios()
        {
            using (ISession session = NHibernateHelper.openSession())
            {
                return session
                    .CreateCriteria<Funcionario>()
                    .List<Funcionario>()
                    .ToList();
            }
        }
    }
}
