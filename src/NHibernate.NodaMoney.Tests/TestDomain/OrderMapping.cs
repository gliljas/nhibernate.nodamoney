using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NodaMoney;
using System;

namespace NHibernate.NodaMoney.Tests.TestDomain
{

    public class OrderMapping : ClassMapping<Order>
    {
        public OrderMapping()
        {
            Table("`Order`");
            Id(x => x.Id, x => x.Generator(Generators.GuidComb));
            Property(x => x.OrderValue, x => {
                x.Type<MoneyType>();
                x.Columns(
                    c => c.Name("OrderValueAmount"),
                    c => c.Name("OrderValueCurrency")
                );
            });
            Property(x => x.OrderValueEUR, x => {
                x.Type<SingleCurrencyMoneyType>(new {Currency="EUR"});
            });
        }
    }
}