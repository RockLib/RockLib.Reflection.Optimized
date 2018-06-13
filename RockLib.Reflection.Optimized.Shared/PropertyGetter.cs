using System;
using System.Linq.Expressions;
using System.Reflection;

namespace RockLib.Reflection.Optimized
{
    internal class PropertyGetter
    {
        internal const string GetValueOptimized = "GetValueOptimized";

        private readonly PropertyInfo _property;
        private Func<object, object> _func;

        public PropertyGetter(PropertyInfo property)
        {
            _property = property;
            _func = _property.GetValue;
        }

        public object GetValue(object obj) => _func.Invoke(obj);

        public void SetOptimizedFunc()
        {
            var objParameter = Expression.Parameter(typeof(object), "obj");

            Expression body = Expression.Call(
                Expression.Convert(objParameter, _property.DeclaringType),
                _property.GetMethod);

            if (_property.PropertyType.IsValueType)
                body = Expression.Convert(body, typeof(object));

            var lambda = Expression.Lambda<Func<object, object>>(body, GetValueOptimized, new[] { objParameter });
            _func = lambda.Compile();
        }

        internal Func<object, object> Func => _func;
    }

    internal class PropertyGetter<TPropertyType>
    {
        private readonly PropertyInfo _property;
        private Func<object, TPropertyType> _func;

        public PropertyGetter(PropertyInfo property)
        {
            _property = property;
            _func = GetValueReflection;
        }

        public TPropertyType GetValue(object obj) => _func.Invoke(obj);

        public void SetOptimizedFunc()
        {
            var objParameter = Expression.Parameter(typeof(object), "obj");

            Expression body = Expression.Call(
                Expression.Convert(objParameter, _property.DeclaringType),
                _property.GetMethod);

            if (_property.PropertyType.IsValueType && !typeof(TPropertyType).IsValueType)
                body = Expression.Convert(body, typeof(TPropertyType));

            var lambda = Expression.Lambda<Func<object, TPropertyType>>(body, PropertyGetter.GetValueOptimized, new[] { objParameter });
            _func = lambda.Compile();
        }

        internal TPropertyType GetValueReflection(object obj) =>
            (TPropertyType)_property.GetValue(obj);

        internal Func<object, TPropertyType> Func => _func;
    }

    internal class PropertyGetter<TDeclaringType, TPropertyType>
    {
        private readonly PropertyInfo _property;
        private Func<TDeclaringType, TPropertyType> _func;

        public PropertyGetter(PropertyInfo property)
        {
            _property = property;
            _func = GetValueReflection;
        }

        public TPropertyType GetValue(TDeclaringType obj) => _func.Invoke(obj);

        public void SetOptimizedFunc()
        {
            var objParameter = Expression.Parameter(typeof(TDeclaringType), "obj");

            Expression body = Expression.Call(objParameter, _property.GetMethod);

            if (_property.PropertyType.IsValueType && !typeof(TPropertyType).IsValueType)
                body = Expression.Convert(body, typeof(TPropertyType));

            var lambda = Expression.Lambda<Func<TDeclaringType, TPropertyType>>(body, PropertyGetter.GetValueOptimized, new[] { objParameter });
            _func = lambda.Compile();
        }

        internal TPropertyType GetValueReflection(TDeclaringType obj) =>
            (TPropertyType)_property.GetValue(obj);

        internal Func<TDeclaringType, TPropertyType> Func => _func;
    }
}
