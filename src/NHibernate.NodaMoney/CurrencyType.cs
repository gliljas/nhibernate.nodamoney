using NodaMoney;
using System;
using NHibernate.Type;
using NHibernate.UserTypes;
using System.Data.Common;
using NHibernate.Engine;
using System.Collections.Generic;

namespace NHibernate.NodaMoney
{
    public class CurrencyType : ICompositeUserType, IParameterizedType
    {
        private static readonly IType _currencyCodeType = TypeFactory.GetAnsiStringType(3);
        public string[] PropertyNames => new[] { nameof(Currency.Code)};

        public IType[] PropertyTypes => new IType[] { _currencyCodeType };

        public System.Type ReturnedClass => typeof(Currency);

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

        public new bool Equals(object? x, object? y)
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

        public int GetHashCode(object? x)
        {
            return x?.GetHashCode() ?? 0;
        }

        public object? GetPropertyValue(object? component, int property)
        {
            if (!(component is Currency currency))
            {
                return null;
            }

            return property switch
            {
                0 => currency.Code,
                _ => throw new ArgumentOutOfRangeException(string.Format("No implementation for property index of '{0}'.", property), nameof(property)),
            };
        }

        public object? NullSafeGet(DbDataReader dr, string[] names, ISessionImplementor session, object owner)
        {
            var currencyCode = (string?)_currencyCodeType.NullSafeGet(dr, names[0], session, owner);
            if (currencyCode == null)
            {
                return null;
            }
            return Currency.FromCode(currencyCode);
        }

        public void NullSafeSet(DbCommand cmd, object value, int index, bool[] settable, ISessionImplementor session)
        {
            _currencyCodeType.NullSafeSet(cmd, (value as Currency?)?.Code, index, settable, session);
        }

        public object Replace(object original, object target, ISessionImplementor session, object owner)
        {
            return original;
        }

        public void SetParameterValues(IDictionary<string, string> parameters)
        {
        }

        public void SetPropertyValue(object component, int property, object value)
        {
            throw new InvalidOperationException("Currency is an immutable object. SetPropertyValue isn't supported.");
        }
    }
}
