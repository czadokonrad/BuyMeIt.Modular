using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BuyMeIt.BuildingBlocks.Domain
{
#nullable enable

    public abstract class ValueObject : IEquatable<ValueObject>
    {
        private List<PropertyInfo>? _properties;
        private List<FieldInfo>? _fields;


        public static bool operator ==(ValueObject? left, ValueObject? right)
        {
            return left?.Equals(right) ?? object.Equals(right, null);
        }

        public static bool operator !=(ValueObject left, ValueObject right) => !(left == right);
        public bool Equals(ValueObject? other)
        {
            return Equals(other as object);
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return GetProperties().All(p => PropertiesAreEqual(obj, p))
                && GetFields().All(f => FieldsAreEqual(obj, f));
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = GetProperties().Select(prop => prop.GetValue(this, null))
                    .Aggregate(17, HashValue);

                return GetFields().Select(field => field.GetValue(this)).Aggregate(hash, HashValue);
            }
        }

        protected static void CheckRule(IBusinessRule rule)
        {
            if (rule.IsBroken())
            {
                throw new BusinessRuleViolationException(rule);
            }
        }

        private bool PropertiesAreEqual(object obj, PropertyInfo p)
        {
            return object.Equals(p.GetValue(this, null), p.GetValue(obj, null));
        }

        private bool FieldsAreEqual(object obj, FieldInfo f)
        {
            return object.Equals(f.GetValue(this), f.GetValue(obj));
        }

        private IEnumerable<PropertyInfo> GetProperties()
        {
            if(_properties == null)
            {
                _properties = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(p => p.GetCustomAttribute(typeof(IgnoreMemberAttribute)) == null)
                    .ToList();
            }

            return _properties;
        }

        private IEnumerable<FieldInfo> GetFields()
        {
            if(_fields == null)
            {
                _fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(p => p.GetCustomAttribute(typeof(IgnoreMemberAttribute)) == null)
                    .ToList();
            }

            return _fields;
        }
        private int HashValue(int seed, object? value)
        {
            var currentHash = value?.GetHashCode() ?? 0;

            return (seed * 23) + currentHash;
        }

    }

#nullable restore
}
