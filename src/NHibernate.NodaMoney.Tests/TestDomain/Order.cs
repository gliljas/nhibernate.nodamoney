using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NodaMoney;
using System;

namespace NHibernate.NodaMoney.Tests.TestDomain
{

    public class Order
    {
        public virtual Guid Id { get; set; }
        public virtual Money OrderValue { get; set; }
        public virtual Money OrderValueEUR { get; set; }
    }
}