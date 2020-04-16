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
    public class CurrencyQueryTests : IDisposable
    {
        private MsSqlNHibernateFixture _fixture;

        public CurrencyQueryTests(MsSqlNHibernateFixture fixture)
        {
            _fixture = fixture;
            using (var session = _fixture.SessionFactory.OpenSession())
            using (var trans = session.BeginTransaction())
            {
                session.Save(new Customer
                {
                    Name = "SwedishCustomer",
                    Currency = Currency.FromCode("SEK")
                });
                session.Save(new Customer
                {
                    Name = "FrenchCustomer",
                    Currency = Currency.FromCode("EUR")
                });
                session.Save(new Customer
                {
                    Name = "GermanCustomer",
                    Currency = Currency.FromCode("EUR")
                });
                trans.Commit();
            }
        }

        [Fact]
        public void Can_Query_By_Currency()
        {
            using var session = _fixture.SessionFactory.OpenSession();
            using var trans = session.BeginTransaction();
            
            var allCustomers = session.Query<Customer>().ToList();
            var sekCustomers = session.Query<Customer>().Where(x => x.Currency == Currency.FromCode("SEK")).ToList();
            var eurCustomers = session.Query<Customer>().Where(x => x.Currency == Currency.FromCode("EUR")).ToList();

            allCustomers.Should().HaveCount(3);
            sekCustomers.Should().HaveCount(1);
            eurCustomers.Should().HaveCount(2);

            trans.Commit();
        }

        [Fact]
        public void Can_Query_By_Currency_Code()
        {
            using var session = _fixture.SessionFactory.OpenSession();
            using var trans = session.BeginTransaction();
            
            var allCustomers = session.Query<Customer>().ToList();
            var swedishCustomers = session.Query<Customer>().Where(x => x.Currency.Code == "SEK").ToList();
            var eurCustomers = session.Query<Customer>().Where(x => x.Currency.Code == "EUR").ToList();

            allCustomers.Should().HaveCount(3);
            swedishCustomers.Should().HaveCount(1);
            eurCustomers.Should().HaveCount(2);

            trans.Commit();
        }

        [Fact]
        public void Can_Group_By_Currency()
        {
            using var session = _fixture.SessionFactory.OpenSession();
            using var trans = session.BeginTransaction();

            var allCustomers = session.Query<Customer>().ToList();
            var customersByCurrency = session.Query<Customer>().GroupBy(x => x.Currency).Select(x => new { Currency = x.Key, Count = x.Count() }).ToList();

            allCustomers.Should().HaveCount(3);
            customersByCurrency.Should().HaveCount(2);
            customersByCurrency.Single(x => x.Currency.Code == "SEK").Count.Should().Be(1);
            customersByCurrency.Single(x => x.Currency.Code == "EUR").Count.Should().Be(2);

            trans.Commit();
        }

        [Fact]
        public void Can_Group_By_Currency_Code()
        {
            using var session = _fixture.SessionFactory.OpenSession();
            using var trans = session.BeginTransaction();

            var allCustomers = session.Query<Customer>().ToList();
            var customersByCurrency = session.Query<Customer>().GroupBy(x => x.Currency.Code).Select(x=>new {CurrencyCode=x.Key,Count=x.Count()}).ToList();
           
            allCustomers.Should().HaveCount(3);
            customersByCurrency.Should().HaveCount(2);
            customersByCurrency.Single(x => x.CurrencyCode == "SEK").Count.Should().Be(1);
            customersByCurrency.Single(x => x.CurrencyCode == "EUR").Count.Should().Be(2);

            trans.Commit();
        }

        public void Dispose()
        {
            _fixture.DeleteAllData();
        }
    }
}
