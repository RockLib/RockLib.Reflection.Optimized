﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace RockLib.Reflection.Optimized
{
#if ROCKLIB_REFLECTION_OPTIMIZED
    internal class PropertyGetter
#else
    partial class PropertyInfoExtensions {
    private class PropertyGetter
#endif
    {
        internal const string GetValueOptimized = nameof(GetValueOptimized);

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

            Expression body;

            if (_property.GetMethod.IsStatic)
                body = Expression.Call(_property.GetMethod);
            else
                body = Expression.Call(
                    Expression.Convert(objParameter, _property.DeclaringType),
                    _property.GetMethod);

            if (_property.PropertyType.IsValueType)
                body = Expression.Convert(body, typeof(object));

            var lambda = Expression.Lambda<Func<object, object>>(body, GetValueOptimized, new[] { objParameter });
            _func = lambda.Compile();
        }

        internal Func<object, object> Func => _func;
    }

#if ROCKLIB_REFLECTION_OPTIMIZED
    internal class PropertyGetter<TPropertyType>
#else
    private class PropertyGetter<TPropertyType>
#endif
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

            Expression body;

            if (_property.GetMethod.IsStatic)
                body = Expression.Call(_property.GetMethod);
            else
                body = Expression.Call(
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

#if ROCKLIB_REFLECTION_OPTIMIZED
    internal class PropertyGetter<TDeclaringType, TPropertyType>
#else
    private class PropertyGetter<TDeclaringType, TPropertyType>
#endif
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

            Expression body;

            if (_property.GetMethod.IsStatic)
                body = Expression.Call(_property.GetMethod);
            else
                body = Expression.Call(objParameter, _property.GetMethod);

            if (_property.PropertyType.IsValueType && !typeof(TPropertyType).IsValueType)
                body = Expression.Convert(body, typeof(TPropertyType));

            var lambda = Expression.Lambda<Func<TDeclaringType, TPropertyType>>(body, PropertyGetter.GetValueOptimized, new[] { objParameter });
            _func = lambda.Compile();
        }

        internal TPropertyType GetValueReflection(TDeclaringType obj) =>
            (TPropertyType)_property.GetValue(obj);

        internal Func<TDeclaringType, TPropertyType> Func => _func;
    }

#if ROCKLIB_REFLECTION_OPTIMIZED
    internal class StaticPropertyGetter
#else
    private class StaticPropertyGetter
#endif
    {
        internal const string GetStaticValueOptimized = nameof(GetStaticValueOptimized);

        private readonly PropertyInfo _property;
        private Func<object> _func;

        public StaticPropertyGetter(PropertyInfo property)
        {
            _property = property;
            _func = GetValueReflection;
        }

        public object GetValue() => _func.Invoke();

        public void SetOptimizedFunc()
        {
            Expression body = Expression.Call(_property.GetMethod);

            if (_property.PropertyType.IsValueType)
                body = Expression.Convert(body, typeof(object));

            var lambda = Expression.Lambda<Func<object>>(body, GetStaticValueOptimized, Enumerable.Empty<ParameterExpression>());
            _func = lambda.Compile();
        }

        internal object GetValueReflection() =>
            _property.GetValue(null);

        internal Func<object> Func => _func;
    }

#if ROCKLIB_REFLECTION_OPTIMIZED
    internal class StaticPropertyGetter<TPropertyType>
#else
    private class StaticPropertyGetter<TPropertyType>
#endif
    {
        private readonly PropertyInfo _property;
        private Func<TPropertyType> _func;

        public StaticPropertyGetter(PropertyInfo property)
        {
            _property = property;
            _func = GetValueReflection;
        }

        public TPropertyType GetValue() => _func.Invoke();

        public void SetOptimizedFunc()
        {
            Expression body = Expression.Call(_property.GetMethod);

            if (_property.PropertyType.IsValueType && !typeof(TPropertyType).IsValueType)
                body = Expression.Convert(body, typeof(TPropertyType));

            var lambda = Expression.Lambda<Func<TPropertyType>>(body, StaticPropertyGetter.GetStaticValueOptimized, Enumerable.Empty<ParameterExpression>());
            _func = lambda.Compile();
        }

        internal TPropertyType GetValueReflection() =>
            (TPropertyType)_property.GetValue(null);

        internal Func<TPropertyType> Func => _func;
    }
#if !ROCKLIB_REFLECTION_OPTIMIZED
    }
#endif
}