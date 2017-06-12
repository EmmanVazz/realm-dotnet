////////////////////////////////////////////////////////////////////////////
//
// Copyright 2017 Realm Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
////////////////////////////////////////////////////////////////////////////

using System;
using Realms.Helpers;

namespace Realms
{
    [Preserve(AllMembers = true, Conditional = false)]
    public struct RealmInteger<T> :
        IEquatable<T>,
        IComparable<RealmInteger<T>>,
        IComparable<T>,
        IConvertible,
        IFormattable
        where T : IComparable, IComparable<T>, IConvertible, IFormattable
    {
        private bool _isManaged;
        private readonly T _value;

        internal RealmInteger(T value)
        {
            _value = value;
            _isManaged = false;
        }

        public RealmInteger<T> Increment()
        {
            return Increment((T)Convert.ChangeType(1, typeof(T)));
        }

        public RealmInteger<T> Decrement()
        {
            return Increment((T)Convert.ChangeType(-1, typeof(T)));
        }

        public RealmInteger<T> Increment(T value)
        {
            if (_isManaged)
            {
                throw new NotImplementedException();
            }

            return new RealmInteger<T>(Operator<T>.Add(value, _value));
        }

        #region Equals

        public override bool Equals(object obj)
        {
            if (obj is RealmInteger<T> realmInteger)
            {
                return Equals(realmInteger);
            }

            if (obj is T value)
            {
                return Equals(value);
            }

            return false;
        }

        public bool Equals(T other)
        {
            return CompareTo(other) == 0;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override string ToString()
        {
            return _value.ToString();
        }

        #endregion

        #region IComparable

        public int CompareTo(RealmInteger<T> other)
        {
            return CompareTo(other._value);
        }

        public int CompareTo(T other)
        {
            return _value.CompareTo(other);
        }

        #endregion

        #region IConvertible

        public TypeCode GetTypeCode() => _value.GetTypeCode();

        public bool ToBoolean(IFormatProvider provider) => _value.ToBoolean(provider);

        public byte ToByte(IFormatProvider provider) => _value.ToByte(provider);

        public char ToChar(IFormatProvider provider) => _value.ToChar(provider);

        public DateTime ToDateTime(IFormatProvider provider) => _value.ToDateTime(provider);

        public decimal ToDecimal(IFormatProvider provider) => _value.ToDecimal(provider);

        public double ToDouble(IFormatProvider provider) => _value.ToDouble(provider);

        public short ToInt16(IFormatProvider provider) => _value.ToInt16(provider);

        public int ToInt32(IFormatProvider provider) => _value.ToInt32(provider);

        public long ToInt64(IFormatProvider provider) => _value.ToInt64(provider);

        public sbyte ToSByte(IFormatProvider provider) => _value.ToSByte(provider);

        public float ToSingle(IFormatProvider provider) => _value.ToSingle(provider);

        public string ToString(IFormatProvider provider) => _value.ToString(provider);

        public object ToType(Type conversionType, IFormatProvider provider) => _value.ToType(conversionType, provider);

        public ushort ToUInt16(IFormatProvider provider) => _value.ToUInt16(provider);

        public uint ToUInt32(IFormatProvider provider) => _value.ToUInt32(provider);

        public ulong ToUInt64(IFormatProvider provider) => _value.ToUInt64(provider);

        #endregion

        #region IFormattable

        public string ToString(string format, IFormatProvider formatProvider) => _value.ToString(format, formatProvider);

        #endregion

        #region Operators

		public static implicit operator T(RealmInteger<T> i)
        {
            return i._value;
        }

        public static implicit operator RealmInteger<T>(T i)
        {
            return new RealmInteger<T>(i);
        }

        public static RealmInteger<T> operator ++(RealmInteger<T> i)
        {
            return i.Increment();
        }

        public static RealmInteger<T> operator --(RealmInteger<T> i)
        {
            return i.Decrement();
        }

        public static bool operator ==(RealmInteger<T> first, RealmInteger<T> second)
        {
            return first.Equals(second);
        }

        public static bool operator !=(RealmInteger<T> first, RealmInteger<T> second)
        {
            return !(first == second);
        }

        #endregion
    }
}