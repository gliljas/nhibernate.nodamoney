using FluentAssertions;
using NHibernate.NodaMoney.Tests.Fixtures;
using NodaMoney;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NHibernate.NodaMoney.Tests
{
    public class AllCurrenciesTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator() => Currency.GetAllCurrencies().Select(x => new object[] { x.Code }).GetEnumerator();


        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
