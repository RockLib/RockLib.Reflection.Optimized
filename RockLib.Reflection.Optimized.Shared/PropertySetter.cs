using System;
using System.Linq.Expressions;
using System.Reflection;

namespace RockLib.Reflection.Optimized
{
#if ROCKLIB_REFLECTION_OPTIMIZED
    internal class PropertySetter
#else
    partial class PropertyInfoExtensions {
    private class PropertySetter
#endif
    {
        internal const string SetValueOptimized = "SetValueOptimized";

        private readonly PropertyInfo _property;
        private Action<object, object> _action;

        public PropertySetter(PropertyInfo property)
        {
            _property = property;
            _action = _property.SetValue;
        }

        public void SetValue(object obj, object value) => _action.Invoke(obj, value);

        public void SetOptimizedAction()
        {
            var objParameter = Expression.Parameter(typeof(object), "obj");
            var valueParameter = Expression.Parameter(typeof(object), "value");

            Expression body;

            if (_property.SetMethod.IsStatic)
                body = Expression.Call(
                    _property.SetMethod,
                    Expression.Convert(valueParameter, _property.PropertyType));
            else
                body = Expression.Call(
                    Expression.Convert(objParameter, _property.DeclaringType),
                    _property.SetMethod,
                    Expression.Convert(valueParameter, _property.PropertyType));

            var lambda = Expression.Lambda<Action<object, object>>(body, SetValueOptimized, new[] { objParameter, valueParameter });
            _action = lambda.Compile();
        }

        internal Action<object, object> Action => _action;
    }

#if ROCKLIB_REFLECTION_OPTIMIZED
    internal class PropertySetter<TPropertyType>
#else
    private class PropertySetter<TPropertyType>
#endif
    {
        private readonly PropertyInfo _property;
        private Action<object, TPropertyType> _action;

        public PropertySetter(PropertyInfo property)
        {
            _property = property;
            _action = SetValueReflection;
        }

        public void SetValue(object obj, TPropertyType value) => _action.Invoke(obj, value);

        public void SetOptimizedAction()
        {
            var objParameter = Expression.Parameter(typeof(object), "obj");
            var valueParameter = Expression.Parameter(typeof(TPropertyType), "value");

            Expression body;

            if (_property.SetMethod.IsStatic)
                body = Expression.Call(
                    _property.SetMethod,
                    valueParameter);
            else
                body = Expression.Call(
                    Expression.Convert(objParameter, _property.DeclaringType),
                    _property.SetMethod,
                    valueParameter);

            var lambda = Expression.Lambda<Action<object, TPropertyType>>(body, PropertySetter.SetValueOptimized, new[] { objParameter, valueParameter });
            _action = lambda.Compile();
        }

        internal void SetValueReflection(object obj, TPropertyType value) =>
            _property.SetValue(obj, value);

        internal Action<object, TPropertyType> Action => _action;
    }

#if ROCKLIB_REFLECTION_OPTIMIZED
    internal class PropertySetter<TDeclaringType, TPropertyType>
#else
    private class PropertySetter<TDeclaringType, TPropertyType>
#endif
    {
        private readonly PropertyInfo _property;
        private Action<TDeclaringType, TPropertyType> _action;

        public PropertySetter(PropertyInfo property)
        {
            _property = property;
            _action = SetValueReflection;
        }

        public void SetValue(TDeclaringType obj, TPropertyType value) => _action.Invoke(obj, value);

        public void SetOptimizedAction()
        {
            var objParameter = Expression.Parameter(typeof(TDeclaringType), "obj");
            var valueParameter = Expression.Parameter(typeof(TPropertyType), "value");

            Expression body;

            if (_property.SetMethod.IsStatic)
                body = Expression.Call(
                    _property.SetMethod,
                    valueParameter);
            else
                body = Expression.Call(
                    objParameter,
                    _property.SetMethod,
                    valueParameter);

            var lambda = Expression.Lambda<Action<TDeclaringType, TPropertyType>>(body, PropertySetter.SetValueOptimized, new[] { objParameter, valueParameter });
            _action = lambda.Compile();
        }

        internal void SetValueReflection(TDeclaringType obj, TPropertyType value) =>
            _property.SetValue(obj, value);

        internal Action<TDeclaringType, TPropertyType> Action => _action;
    }
#if !ROCKLIB_REFLECTION_OPTIMIZED
    }
#endif
    }
