using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NodaMoney;
using System;

namespace NHibernate.NodaMoney.Tests.TestDomain
{

    public class CustomerMapping : ClassMapping<Customer>
    {
        public CustomerMapping()
        {
            Table("Customer");
            Id(x => x.Id, x => x.Generator(Generators.GuidComb));
            Property(x => x.Name);
            Property(x => x.Currency, x => x.Type<CurrencyType>(new {}));
        }
    }
}