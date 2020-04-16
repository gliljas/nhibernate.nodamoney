using NodaMoney;
using System;

namespace NHibernate.NodaMoney.Tests.TestDomain
{
    public class Customer
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual Currency Currency { get; set; }
    }
}