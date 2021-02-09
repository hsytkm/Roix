using System;
using System.Linq;
using System.Linq.Expressions;

namespace Roix.Core
{
    // C# のジェネリック演算 https://qiita.com/Zuishin/items/61fc8807d027d5cea329
    sealed class GenericOperation<T> where T : struct
    {
        private static readonly GenericOperation<T> _instance = new GenericOperation<T>();
        public static GenericOperation<T> GetInstance() => _instance;

        private readonly ParameterExpression _p1;
        private readonly ParameterExpression _p2;

        private GenericOperation()
        {
            var type = typeof(T);
            if (!AvailableTypes.Contains(type)) throw new NotSupportedException(type.FullName);

            _p1 = Expression.Parameter(type);
            _p2 = Expression.Parameter(type);
        }

        private Type[] AvailableTypes => _availableTypes ??= new[]
        {
            typeof(int), typeof(double),
            //typeof(sbyte), typeof(short), typeof(int), typeof(long),
            //typeof(byte), typeof(ushort), typeof(uint), typeof(ulong),
            //typeof(char), typeof(decimal), typeof(Half), typeof(float), typeof(double)
        };
        private Type[]? _availableTypes = null;

        public Func<T, T, T> Add => _add ??= Expression.Lambda<Func<T, T, T>>(Expression.Add(_p1, _p2), _p1, _p2).Compile();
        private Func<T, T, T>? _add = default;

        public Func<T, T, T> Subtract => _subtract ??= Expression.Lambda<Func<T, T, T>>(Expression.Subtract(_p1, _p2), _p1, _p2).Compile();
        private Func<T, T, T>? _subtract = default;

        public Func<T, T, bool> GreaterThanOrEqual => _greaterThanOrEqual ??= Expression.Lambda<Func<T, T, bool>>(Expression.GreaterThanOrEqual(_p1, _p2), _p1, _p2).Compile();
        private Func<T, T, bool>? _greaterThanOrEqual = default;
        public Func<T, T, bool> LessThanOrEqual => _lessThanOrEqual ??= Expression.Lambda<Func<T, T, bool>>(Expression.LessThanOrEqual(_p1, _p2), _p1, _p2).Compile();
        private Func<T, T, bool>? _lessThanOrEqual = default;

        public T Clamp(T value, T min, T max)
        {
            if (LessThanOrEqual(value, min)) return min;
            if (GreaterThanOrEqual(value, max)) return max;
            return value;
        }

    }
}
