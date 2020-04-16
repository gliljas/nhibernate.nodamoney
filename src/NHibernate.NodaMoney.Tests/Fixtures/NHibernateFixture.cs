using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Threading.Tasks;
using Xunit;

namespace NHibernate.NodaMoney.Tests.Fixtures
{

    public abstract class NHibernateFixture 
    {
        private Lazy<Configuration> _configuration;
        private Lazy<ISessionFactory> _sessionFactory;

        protected NHibernateFixture() 
        {
            _configuration = new Lazy<Configuration>(()=> {
                var config = new Configuration();
                Configure(config);
                var schemaExport = new SchemaUpdate(config);
                schemaExport.Execute(true, true);
                return config;
            });
            _sessionFactory = new Lazy<ISessionFactory>(() => _configuration.Value.BuildSessionFactory());
        }
        protected abstract void Configure(Configuration configuration);
        public ISessionFactory SessionFactory => _sessionFactory.Value;

        public virtual void DeleteAllData()
        {
            if (_sessionFactory.IsValueCreated)
            {
                using (var session = _sessionFactory.Value.OpenSession())
                {
                    using (var trans = session.BeginTransaction())
                    {
                        session.Delete("from System.Object");
                        trans.Commit();
                    }
                }
            }
        }
    }
}
