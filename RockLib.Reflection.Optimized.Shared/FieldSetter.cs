using System;
using System.Linq.Expressions;
using System.Reflection;

namespace RockLib.Reflection.Optimized
{
#if ROCKLIB_REFLECTION_OPTIMIZED
    internal class FieldSetter
#else
    partial class FieldInfoExtensions {
    private class FieldSetter
#endif
    {
        internal const string SetValueOptimized = nameof(SetValueOptimized);

        private readonly FieldInfo _field;
        private Action<object, object> _action;

        public FieldSetter(FieldInfo field)
        {
            _field = field;
            _action = _field.SetValue;
        }

        public void SetValue(object obj, object value) => _action.Invoke(obj, value);

        public void SetOptimizedAction()
        {
            var objParameter = Expression.Parameter(typeof(object), "obj");
            var valueParameter = Expression.Parameter(typeof(object), "value");

            Expression body;

            if (_field.IsStatic)
                body = Expression.Assign(
                    Expression.Field(null, _field),
                    Expression.Convert(valueParameter, _field.FieldType));
            else
                body = Expression.Assign(
                    Expression.Field(
                        Expression.Convert(objParameter, _field.DeclaringType),
                        _field),
                    Expression.Convert(valueParameter, _field.FieldType));

            var lambda = Expression.Lambda<Action<object, object>>(body, SetValueOptimized, new[] { objParameter, valueParameter });
            _action = lambda.Compile();
        }

        internal Action<object, object> Action => _action;
    }

#if ROCKLIB_REFLECTION_OPTIMIZED
    internal class FieldSetter<TFieldType>
#else
    private class FieldSetter<TFieldType>
#endif
    {
        private readonly FieldInfo _field;
        private Action<object, TFieldType> _action;

        public FieldSetter(FieldInfo field)
        {
            _field = field;
            _action = SetValueReflection;
        }

        public void SetValue(object obj, TFieldType value) => _action.Invoke(obj, value);

        public void SetOptimizedAction()
        {
            var objParameter = Expression.Parameter(typeof(object), "obj");
            var valueParameter = Expression.Parameter(typeof(TFieldType), "value");

            Expression body;

            if (_field.IsStatic)
                body = Expression.Assign(
                    Expression.Field(null, _field),
                    valueParameter);
            else
                body = Expression.Assign(
                    Expression.Field(
                        Expression.Convert(objParameter, _field.DeclaringType),
                        _field),
                    valueParameter);

            var lambda = Expression.Lambda<Action<object, TFieldType>>(body, FieldSetter.SetValueOptimized, new[] { objParameter, valueParameter });
            _action = lambda.Compile();
        }

        internal void SetValueReflection(object obj, TFieldType value) =>
            _field.SetValue(obj, value);

        internal Action<object, TFieldType> Action => _action;
    }

#if ROCKLIB_REFLECTION_OPTIMIZED
    internal class FieldSetter<TDeclaringType, TFieldType>
#else
    private class FieldSetter<TDeclaringType, TFieldType>
#endif
    {
        private readonly FieldInfo _field;
        private Action<TDeclaringType, TFieldType> _action;

        public FieldSetter(FieldInfo field)
        {
            _field = field;
            _action = SetValueReflection;
        }

        public void SetValue(TDeclaringType obj, TFieldType value) => _action.Invoke(obj, value);

        public void SetOptimizedAction()
        {
            var objParameter = Expression.Parameter(typeof(TDeclaringType), "obj");
            var valueParameter = Expression.Parameter(typeof(TFieldType), "value");

            Expression body;

            if (_field.IsStatic)
                body = Expression.Assign(
                    Expression.Field(null, _field),
                    valueParameter);
            else
                body = Expression.Assign(
                    Expression.Field(objParameter, _field),
                    valueParameter);

            var lambda = Expression.Lambda<Action<TDeclaringType, TFieldType>>(body, FieldSetter.SetValueOptimized, new[] { objParameter, valueParameter });
            _action = lambda.Compile();
        }

        internal void SetValueReflection(TDeclaringType obj, TFieldType value) =>
            _field.SetValue(obj, value);

        internal Action<TDeclaringType, TFieldType> Action => _action;
    }

#if ROCKLIB_REFLECTION_OPTIMIZED
    internal class StaticFieldSetter
#else
    private class StaticFieldSetter
#endif
    {
        internal const string SetStaticValueOptimized = nameof(SetStaticValueOptimized);

        private readonly FieldInfo _field;
        private Action<object> _action;

        public StaticFieldSetter(FieldInfo field)
        {
            _field = field;
            _action = SetValueReflection;
        }

        public void SetValue(object value) => _action.Invoke(value);

        public void SetOptimizedAction()
        {
            var valueParameter = Expression.Parameter(typeof(object), "value");

            Expression body = Expression.Assign(
                Expression.Field(null, _field),
                Expression.Convert(valueParameter, _field.FieldType));

            var lambda = Expression.Lambda<Action<object>>(body, SetStaticValueOptimized, new[] { valueParameter });
            _action = lambda.Compile();
        }

        internal void SetValueReflection(object value) =>
            _field.SetValue(null, value);

        internal Action<object> Action => _action;
    }

#if ROCKLIB_REFLECTION_OPTIMIZED
    internal class StaticFieldSetter<TFieldType>
#else
    private class StaticFieldSetter<TFieldType>
#endif
    {
        private readonly FieldInfo _field;
        private Action<TFieldType> _action;

        public StaticFieldSetter(FieldInfo field)
        {
            _field = field;
            _action = SetValueReflection;
        }

        public void SetValue(TFieldType value) => _action.Invoke(value);

        public void SetOptimizedAction()
        {
            var valueParameter = Expression.Parameter(typeof(TFieldType), "value");

            Expression body = Expression.Assign(
                Expression.Field(null, _field),
                valueParameter);

            var lambda = Expression.Lambda<Action<TFieldType>>(body, StaticFieldSetter.SetStaticValueOptimized, new[] { valueParameter });
            _action = lambda.Compile();
        }

        internal void SetValueReflection(TFieldType value) =>
            _field.SetValue(null, value);

        internal Action<TFieldType> Action => _action;
    }
#if !ROCKLIB_REFLECTION_OPTIMIZED
    }
#endif
}
