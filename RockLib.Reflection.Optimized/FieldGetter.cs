using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace RockLib.Reflection.Optimized
{
    internal class FieldGetter
    {
        internal const string GetValueOptimized = nameof(GetValueOptimized);

        private readonly FieldInfo _field;
        private Func<object, object?> _func;

        public FieldGetter(FieldInfo field)
        {
            _field = field ?? throw new ArgumentNullException(nameof(field));
            _func = _field.GetValue;
        }

        public object GetValue(object obj) => _func.Invoke(obj)!;

        public void SetOptimizedFunc()
        {
            var objParameter = Expression.Parameter(typeof(object), "obj");

            Expression body;

            if (_field.IsStatic)
            {
                body = Expression.Field(null, _field);
            }
            else if (_field.DeclaringType != null)
            {
                body = Expression.Field(
                    Expression.Convert(objParameter, _field.DeclaringType),
                    _field);
            }

            if (_field.FieldType.IsValueType)
            {
                body = Expression.Convert(body, typeof(object));
            }

            var lambda = Expression.Lambda<Func<object, object>>(body, GetValueOptimized, new[] { objParameter });
            _func = lambda.Compile();
        }

        internal Func<object, object?> Func => _func;
    }

    internal class FieldGetter<TFieldType>
    {
        private readonly FieldInfo _field;
        private Func<object, TFieldType> _func;

        public FieldGetter(FieldInfo field)
        {
            _field = field ?? throw new ArgumentNullException(nameof(field));
            _func = GetValueReflection;
        }

        public TFieldType GetValue(object obj) => _func.Invoke(obj);

        public void SetOptimizedFunc()
        {
            var objParameter = Expression.Parameter(typeof(object), "obj");

            Expression body;

            if (_field.IsStatic)
            {
                body = Expression.Field(null, _field);
            }
            else if (_field.DeclaringType != null)
            {
                body = Expression.Field(
                    Expression.Convert(objParameter, _field.DeclaringType),
                    _field);
            }

            if (_field.FieldType.IsValueType && !typeof(TFieldType).IsValueType)
            {
                body = Expression.Convert(body, typeof(TFieldType));
            }

            var lambda = Expression.Lambda<Func<object, TFieldType>>(body, FieldGetter.GetValueOptimized, new[] { objParameter });
            _func = lambda.Compile();
        }

        internal TFieldType GetValueReflection(object obj) =>
            (TFieldType)_field.GetValue(obj)!;

        internal Func<object, TFieldType> Func => _func;
    }

    internal class FieldGetter<TDeclaringType, TFieldType>
    {
        private readonly FieldInfo _field;
        private Func<TDeclaringType, TFieldType> _func;

        public FieldGetter(FieldInfo field)
        {
            _field = field;
            _func = GetValueReflection;
        }

        public TFieldType GetValue(TDeclaringType obj) => _func.Invoke(obj);

        public void SetOptimizedFunc()
        {
            var objParameter = Expression.Parameter(typeof(TDeclaringType), "obj");

            Expression body;

            if (_field.IsStatic)
                body = Expression.Field(null, _field);
            else
                body = Expression.Field(objParameter, _field);

            if (_field.FieldType.IsValueType && !typeof(TFieldType).IsValueType)
                body = Expression.Convert(body, typeof(TFieldType));

            var lambda = Expression.Lambda<Func<TDeclaringType, TFieldType>>(body, FieldGetter.GetValueOptimized, new[] { objParameter });
            _func = lambda.Compile();
        }

        internal TFieldType GetValueReflection(TDeclaringType obj) =>
            (TFieldType)_field.GetValue(obj)!;

        internal Func<TDeclaringType, TFieldType> Func => _func;
    }

    internal class StaticFieldGetter
    {
        internal const string GetStaticValueOptimized = nameof(GetStaticValueOptimized);

        private readonly FieldInfo _field;
        private Func<object> _func;

        public StaticFieldGetter(FieldInfo field)
        {
            _field = field;
            _func = GetValueReflection;
        }

        public object GetValue() => _func.Invoke();

        public void SetOptimizedFunc()
        {
            Expression body = Expression.Field(null, _field);

            if (_field.FieldType.IsValueType)
                body = Expression.Convert(body, typeof(object));

            var lambda = Expression.Lambda<Func<object>>(body, GetStaticValueOptimized, Enumerable.Empty<ParameterExpression>());
            _func = lambda.Compile();
        }

        internal object GetValueReflection() =>
            _field.GetValue(null)!;

        internal Func<object> Func => _func;
    }

    internal class StaticFieldGetter<TFieldType>
    {
        private readonly FieldInfo _field;
        private Func<TFieldType> _func;

        public StaticFieldGetter(FieldInfo field)
        {
            _field = field;
            _func = GetValueReflection;
        }

        public TFieldType GetValue() => _func.Invoke();

        public void SetOptimizedFunc()
        {
            Expression body = Expression.Field(null, _field);

            if (_field.FieldType.IsValueType && !typeof(TFieldType).IsValueType)
                body = Expression.Convert(body, typeof(TFieldType));

            var lambda = Expression.Lambda<Func<TFieldType>>(body, StaticFieldGetter.GetStaticValueOptimized, Enumerable.Empty<ParameterExpression>());
            _func = lambda.Compile();
        }

        internal TFieldType GetValueReflection() =>
            (TFieldType)_field.GetValue(null)!;

        internal Func<TFieldType> Func => _func;
    }
}
