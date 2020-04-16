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
    public class CurrencyPersistenceTests : IDisposable
    {
        private MsSqlNHibernateFixture _fixture;

        public CurrencyPersistenceTests(MsSqlNHibernateFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [ClassData(typeof(AllCurrenciesTestData))]
        public void Can_Save_Currency(string currencyCode)
        {
            var testCurrency = Currency.FromCode(currencyCode);
            Guid customerId;
            using (var session = _fixture.SessionFactory.OpenSession())
            using (var trans = session.BeginTransaction())
            {
                var customer = new Customer
                {
                    Name = currencyCode,
                    Currency = testCurrency
                };
                session.Save(customer);
                customerId = customer.Id;
                trans.Commit();
            }

            using (var session = _fixture.SessionFactory.OpenSession())
            using (var trans = session.BeginTransaction())
            {
                var customer = session.Get<Customer>(customerId);
                customer.Currency.Should().Be(testCurrency);
            }
        }

        public void Dispose()
        {
            _fixture.DeleteAllData();
        }
    }
}
