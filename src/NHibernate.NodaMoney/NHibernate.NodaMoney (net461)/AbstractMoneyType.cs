using NHibernate.Engine;
using NHibernate.Type;
using NHibernate.UserTypes;
using NodaMoney;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace NHibernate.NodaMoney
{
    public abstract class AbstractMoneyType : ICompositeUserType, IParameterizedType
    {
        private readonly CompositeCustomType CurrencyCustomType = new CompositeCustomType(typeof(CurrencyType), new Dictionary<string, string>());
        public string[] PropertyNames => new string[] { nameof(Money.Amount), nameof(Money.Currency) };

        public IType[] PropertyTypes => new IType[] { NHibernateUtil.Decimal, CurrencyCustomType };

        public System.Type ReturnedClass => typeof(Money);

        public bool IsMutable => false;

        public object Assemble(object cached, ISessionImplementor session, object owner)
        {
            return cached;
        }

        public object DeepCopy(object value)
        {
            return value;
        }

        public object Disassemble(object value, ISessionImplementor session)
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
        public object? GetPropertyValue(object? component, int property)
        {
            if (!(component is Money money))
            {
                return null;
            }

            return property switch
            {
                0 => money.Amount,
                1 => money.Currency,
                _ => throw new ArgumentOutOfRangeException(string.Format("No implementation for property index of '{0}'.", property), nameof(property)),
            };
        }

        public object? NullSafeGet(DbDataReader dr, string[] names, ISessionImplementor session, object owner)
        {
            var amount = (decimal?)NHibernateUtil.Decimal.NullSafeGet(dr, names[0], session, owner);
            if (amount != null)
            {
                var currency = (Currency?)CurrencyCustomType.NullSafeGet(dr, names[1], session, owner);
                if (currency != null)
                {
                    return new Money(AdjustValueFromPersistence(amount.Value, currency.Value), currency.Value);
                }
            }
            return null;
        }

        public void NullSafeSet(DbCommand cmd, object value, int index, bool[] settable, ISessionImplementor session)
        {
            if (value == null)
            {
                NHibernateUtil.Decimal.NullSafeSet(cmd, null, index, session);
                CurrencyCustomType.NullSafeSet(cmd, null, index + 1, session);
            }
            else
            {
                var money = (Money)value;
                NHibernateUtil.Decimal.NullSafeSet(cmd, AdjustValueForPersistence(money.Amount, money.Currency), index, session);
                CurrencyCustomType.NullSafeSet(cmd, money.Currency, index + 1, session);
            }
        }

        public object Replace(object original, object target, ISessionImplementor session, object owner)
        {
            return original;
        }

        public void SetParameterValues(IDictionary<string, string> parameters)
        {
            if (parameters != null)
            {
                var currencyParameters = new Dictionary<string, string>();
                foreach (var parameter in parameters)
                {
                    if (parameter.Key.StartsWith("CurrencyType"))
                    {
                        currencyParameters.Add(parameter.Key.Remove("CurrencyType".Length), parameter.Value);
                        continue;
                    }
                }
                if (currencyParameters.Count > 0)
                {
                    ((CurrencyType)CurrencyCustomType.UserType).SetParameterValues(currencyParameters);
                }
            }
        }

        public void SetPropertyValue(object component, int property, object value)
        {
            throw new InvalidOperationException("Money is an immutable object. SetPropertyValue isn't supported.");
        }

        protected virtual decimal AdjustValueForPersistence(decimal amount, Currency currency)
        {
            return amount;
        }

        protected virtual decimal AdjustValueFromPersistence(decimal amount, Currency currency)
        {
            return amount;
        }
    }


}
