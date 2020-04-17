using Microsoft.Extensions.Configuration;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using NHibernate.NodaMoney.Tests.TestDomain;
using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernate.NodaMoney.Tests.Fixtures
{
    public class BlappFixture : NHibernateFixture
    {
        public BlappFixture()
        {
            
        }

        protected override void Configure(Configuration configuration)
        {
            var root = TestHelper
                .GetIConfigurationRoot();

            var nhibernate = root
                .GetSection("nhibernate");

            nhibernate
                .Bind(configuration.Properties);

            var mapper = new ModelMapper();
            mapper.AddMapping<CustomerMapping>();
            mapper.AddMapping<OrderMapping>();
            var mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
            configuration.AddMapping(mapping);
        }
    }
}
