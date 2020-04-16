using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using NHibernate.NodaMoney.Tests.TestDomain;
using System.Threading.Tasks;
using Xunit;

namespace NHibernate.NodaMoney.Tests.Fixtures
{

    public class MsSqlNHibernateFixture : NHibernateFixture, IAsyncLifetime
    {
        private readonly MsSqlContainerFixture _sqlFixture;

        public MsSqlNHibernateFixture()
        {
            _sqlFixture = new MsSqlContainerFixture();
        }

        public async Task DisposeAsync()
        {
            await _sqlFixture.DisposeAsync();
        }

        public async Task InitializeAsync()
        {
            await _sqlFixture.InitializeAsync();
        }

        protected override void Configure(Configuration configuration)
        {
            configuration.DataBaseIntegration(db =>
            {
                db.Dialect<MsSql2012Dialect>();
                db.ConnectionString = _sqlFixture.Container.GetConnectionString("TestDatabase");
                db.BatchSize = 100;
            });

            var mapper = new ModelMapper();
            mapper.AddMapping<CustomerMapping>();
            mapper.AddMapping<OrderMapping>();
            HbmMapping mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
            configuration.AddMapping(mapping);
        }
    }
}
