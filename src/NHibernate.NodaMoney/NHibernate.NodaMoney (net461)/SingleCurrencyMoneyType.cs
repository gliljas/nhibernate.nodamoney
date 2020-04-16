using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.Type;
using NHibernate.UserTypes;
using NodaMoney;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace NHibernate.NodaMoney
{

    public class SingleCurrencyMoneyType : IUserType, IParameterizedType
    {
        private Currency _currency;

        public SqlType[] SqlTypes => new[] { SqlTypeFactory.Decimal };

        public System.Type ReturnedType => typeof(Money);

        public bool IsMutable => false;

        public object Assemble(object cached, object owner)
        {
            return cached;
        }

        public object DeepCopy(object value)
        {
            return value;
        }

        public object Disassemble(object value)
        {
            return value;
        }

        public new bool Equals(object x, object y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return x.Equals(y);
        }

        public int GetHashCode(object x)
        {
            return x?.GetHashCode() ?? 0;
        }

        public object? NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
        {
            var amount = (decimal?)NHibernateUtil.Decimal.NullSafeGet(rs, names[0], session, owner);
            if (amount != null)
            {
                return new Money(amount.Value, _currency);
            }
            return null;
        }

        public void NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor session)
        {
            if (value == null)
            {
                NHibernateUtil.Decimal.NullSafeSet(cmd, null, index, session);
            }
            else
            {
                var money = (Money)value;
                if (money.Currency != default && money.Currency != _currency)
                {
                    throw new ArgumentException($"Only {nameof(Money)} with currency {_currency.Code} are allowed.");
                }
                NHibernateUtil.Decimal.NullSafeSet(cmd, money.Amount, index, session);
            }
        }

        public object Replace(object original, object target, object owner)
        {
            return original;
        }

        public void SetParameterValues(IDictionary<string, string> parameters)
        {
            if (parameters != null)
            {
                if (parameters.TryGetValue("Currency", out var currencyCode))
                {
                    _currency = Currency.FromCode(currencyCode);
                    return;
                }
            }
            throw new ArgumentException($"{nameof(SingleCurrencyMoneyType)} msut be configured with a valid Currency setting");
        }
    }


}
