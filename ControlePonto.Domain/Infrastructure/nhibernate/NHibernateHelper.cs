using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using ControlePonto.Domain.usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Tool.hbm2ddl;
using MySql.Data.MySqlClient;
using NHibernate.Context;

namespace ControlePonto.Infrastructure.nhibernate
{
    public class NHibernateHelper
    {
        private static FluentConfiguration _fluentConfiguration;
        private static ISessionFactory _sessionFactory;

        private static string _host = "localhost";
        public static string Host
        {
            get { return _host; }
            set 
            {
                if (sessionFactoryWasCreated())
                    throw new InvalidOperationException("Não é possível trocar o host. É necessário reiniciar a aplicação");

                _host = value;
                try
                {
                    createSessionFactory();
                }
                catch(MySqlException ex)
                {
                    throw new InvalidHostException(value, ex);
                }
            }
        }

        private static string ConnectionString
        {
#if DEBUG
            get { return string.Format("Server={0};Database=db_ponto_artplas;User ID=root;Password=root;", Host); }
#else
            get { return string.Format("Server={0};Database=db_ponto_artplas;User ID=artplas_app_adm;Password=PLACEHOLDER;", Host); }
#endif
        }

        public static FluentConfiguration getFluentConfiguration()
        {
            if (_fluentConfiguration == null)
            {
                _fluentConfiguration = Fluently.Configure()
                    .Database(MySQLConfiguration.Standard.ConnectionString(ConnectionString))
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<ControlePonto.Infrastructure.nhibernate.mapping.UsuarioMap>())
                    .ExposeConfiguration(c => new SchemaUpdate(c).Execute(false, true));
                //One caveat: SchemaUpdate does not do destructive updates (dropping tables, columns, etc.). It will only add them.
            }
            return _fluentConfiguration;
        }

        //public static FluentConfiguration getFluentConfiguration()
        //{
        //    if (_fluentConfiguration == null)
        //    {
        //        _fluentConfiguration = Fluently.Configure()
        //            .Database(SQLiteConfiguration.Standard.UsingFile("controle-ponto.db"))
        //            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<ControlePonto.Infrastructure.nhibernate.mapping.UsuarioMap>())
        //            .ExposeConfiguration(c => new SchemaUpdate(c).Execute(false, true))
        //            .CurrentSessionContext<ThreadStaticSessionContext>();
        //    }
        //    return _fluentConfiguration;
        //}

        public static ISessionFactory getSessionFactory()
        {
            if (!sessionFactoryWasCreated())
                createSessionFactory();            
            return _sessionFactory;
        }

        private static void createSessionFactory()
        {
            _sessionFactory =  getFluentConfiguration().BuildSessionFactory();
        }

        private static bool sessionFactoryWasCreated()
        {
            return _sessionFactory != null;
        }

        public static ISession openSession()
        {
            return getSessionFactory().OpenSession();
        }        
    }
}
