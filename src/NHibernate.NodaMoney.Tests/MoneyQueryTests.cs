using FluentAssertions;
using NHibernate.NodaMoney.Tests.Fixtures;
using NHibernate.NodaMoney.Tests.TestDomain;
using NodaMoney;
using System;
using System.Linq;
using Xunit;

namespace NHibernate.NodaMoney.Tests
{
    [Collection("Database tests")]
    public class MoneyQueryTests : IDisposable
    {
        private BlappFixture _fixture;

        public MoneyQueryTests(BlappFixture fixture)
        {
            _fixture = fixture;
            using (var session = _fixture.SessionFactory.OpenSession())
            using (var trans = session.BeginTransaction())
            {
                session.Save(new Order
                {
                    OrderValue = new Money(100,"SEK")
                });
                session.Save(new Order
                {
                    OrderValue = new Money(100, "EUR")
                });
                session.Save(new Order
                {
                    OrderValue = new Money(100, "EUR")
                });
                trans.Commit();
            }
        }

        [Fact]
        public void Can_Query_By_Currency()
        {
            using var session = _fixture.SessionFactory.OpenSession();
            using var trans = session.BeginTransaction();
            
            var allOrders = session.Query<Order>().ToList();
            var sekOrders = session.Query<Order>().Where(x => x.OrderValue.Currency == Currency.FromCode("SEK")).ToList();
            var eurOrders = session.Query<Order>().Where(x => x.OrderValue.Currency == Currency.FromCode("EUR")).ToList();

            allOrders.Should().HaveCount(3);
            sekOrders.Should().HaveCount(1);
            eurOrders.Should().HaveCount(2);

            trans.Commit();
        }

        //[Fact]
        //public void Can_Query_By_Currency_Code()
        //{
        //    using var session = _fixture.SessionFactory.OpenSession();
        //    using var trans = session.BeginTransaction();

        //    var allOrders = session.Query<Order>().ToList();
        //    var swedishOrders = session.Query<Order>().Where(x => x.OrderValue.Currency.Code == "SEK").ToList();
        //    var eurOrders = session.Query<Order>().Where(x => x.OrderValue.Currency.Code == "EUR").ToList();

        //    allOrders.Should().HaveCount(3);
        //    swedishOrders.Should().HaveCount(1);
        //    eurOrders.Should().HaveCount(2);

        //    trans.Commit();
        //}

        [Fact]
        public void Can_Group_By_Currency()
        {
            using var session = _fixture.SessionFactory.OpenSession();
            using var trans = session.BeginTransaction();

            var allOrders = session.Query<Order>().ToList();
            var OrdersByCurrency = session.Query<Order>().GroupBy(x => x.OrderValue.Currency).Select(x => new { Currency = x.Key, Count = x.Count(), Sum = x.Sum(y=>y.OrderValue.Amount) }).ToList();

            allOrders.Should().HaveCount(3);
            OrdersByCurrency.Should().HaveCount(2);
            OrdersByCurrency.Single(x => x.Currency.Code == "SEK").Count.Should().Be(1);
            OrdersByCurrency.Single(x => x.Currency.Code == "SEK").Sum.Should().Be(100);
            OrdersByCurrency.Single(x => x.Currency.Code == "EUR").Count.Should().Be(2);
            OrdersByCurrency.Single(x => x.Currency.Code == "EUR").Sum.Should().Be(200);

            trans.Commit();
        }

        //[Fact]
        //public void Can_Group_By_Currency_Code()
        //{
        //    using var session = _fixture.SessionFactory.OpenSession();
        //    using var trans = session.BeginTransaction();

        //    var allOrders = session.Query<Customer>().ToList();
        //    var OrdersByCurrency = session.Query<Customer>().GroupBy(x => x.Currency.Code).Select(x=>new {CurrencyCode=x.Key,Count=x.Count()}).ToList();

        //    allOrders.Should().HaveCount(3);
        //    OrdersByCurrency.Should().HaveCount(2);
        //    OrdersByCurrency.Single(x => x.CurrencyCode == "SEK").Count.Should().Be(1);
        //    OrdersByCurrency.Single(x => x.CurrencyCode == "EUR").Count.Should().Be(2);

        //    trans.Commit();
        //}

        public void Dispose()
        {
            _fixture.DeleteAllData();
        }
    }
}
