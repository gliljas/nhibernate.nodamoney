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

    [CollectionDefinition("Database tests")]
    public class DatabaseCollection : ICollectionFixture<MsSqlNHibernateFixture>
    {

    }
}
