using FluentAssertions;
using NHibernate.NodaMoney.Tests.Fixtures;
using NHibernate.NodaMoney.Tests.TestDomain;
using NodaMoney;
using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace NHibernate.NodaMoney.Tests
{
    [Collection("Database tests")]
    public class MoneyPersistenceTests : IDisposable
    {
        private MsSqlNHibernateFixture _fixture;

        public MoneyPersistenceTests(MsSqlNHibernateFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Can_Save_Money()
        {
            Guid orderId;
            using (var session = _fixture.SessionFactory.OpenSession())
            using (var trans = session.BeginTransaction())
            {
                var order = new Order
                {
                    OrderValue = Money.Euro(100)
                };
                session.Save(order);
                orderId = order.Id;
                trans.Commit();
            }

            using (var session = _fixture.SessionFactory.OpenSession())
            using (var trans = session.BeginTransaction())
            {
                var order = session.Get<Order>(orderId);
                order.OrderValue.Should().Be(Money.Euro(100));
            }
        }

        public void Dispose()
        {
            _fixture.DeleteAllData();
        }
    }
}
