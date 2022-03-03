using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.services.persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        public NHibernate.ISession Session { get; private set; }
        private NHibernate.ISessionFactory sessionFactory;

        public UnitOfWork(NHibernate.ISessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }

        public bool IsOpen
        {
            get
            {
                if (Session != null && Session.IsOpen)
                    return true;
                return false;
            }
        }

        protected UnitOfWork() { }

        public virtual ITransaction beginTransaction()
        {
            return Session.BeginTransaction();
        }

        public virtual void openConnection()
        {
            Session = sessionFactory.OpenSession();
        }

        public virtual void closeConnection()
        {
            if (Session.IsOpen)
                Session.Close();
        }

        public virtual void flush()
        {
            Session.Flush();
            Session.Clear();
        }

        public virtual void Dispose()
        {
            if (Session != null)
                Session.Dispose();
        }
    }
}
