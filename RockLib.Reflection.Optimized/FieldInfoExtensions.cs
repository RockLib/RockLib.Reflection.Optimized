using System;
using System.Reflection;
using System.Threading;

namespace RockLib.Reflection.Optimized
{
    /// <summary>
    /// Defines extension methods that return <see cref="Func{T, TResult}"/> or <see cref="Action{T1, T2}"/> delegates
    /// that get or set a field value of an object. These delegates provide much faster access than using reflection.
    /// </summary>
    public static class FieldInfoExtensions
    {
        /// <summary>
        /// Creates a function that gets the value of the specified field. The parameter of the
        /// resulting function takes the object whose field value is to be accessed. If the
        /// specified field is static, the parameter of the resulting function is ignored.
        /// </summary>
        /// <param name="field">The field to create the getter function for.</param>
        /// <returns>A function that gets the field value.</returns>
        public static Func<object, object> CreateGetter(this FieldInfo field)
        {
            if (field is null)
                throw new ArgumentNullException(nameof(field));

            var getter = new FieldGetter(field);
            QueueUserWorkItem(getter, g => g.SetOptimizedFunc());
            return getter.GetValue;
        }

        /// <summary>
        /// Creates a function that gets the value of the specified field. The parameter of the
        /// resulting function takes the object whose field value is to be accessed. If the
        /// specified field is static, the parameter of the resulting function is ignored.
        /// </summary>
        /// <typeparam name="TFieldType">
        /// The return type of the resulting function. This type must be compatible with the
        /// <see cref="FieldInfo.FieldType"/> of the <paramref name="field"/> parameter.
        /// </typeparam>
        /// <param name="field">The field to create the getter function for.</param>
        /// <returns>A function that gets the field value.</returns>
        public static Func<object, TFieldType> CreateGetter<TFieldType>(this FieldInfo field)
        {
            if (field is null)
                throw new ArgumentNullException(nameof(field));
            if (!typeof(TFieldType).IsAssignableFrom(field.FieldType))
                throw new ArgumentException("TFieldType must be assignable from field.FieldType", nameof(field));

            var getter = new FieldGetter<TFieldType>(field);
            QueueUserWorkItem(getter, g => g.SetOptimizedFunc());
            return getter.GetValue;
        }

        /// <summary>
        /// Creates a function that gets the value of the specified field. The parameter of the
        /// resulting function takes the object whose field value is to be accessed. If the
        /// specified field is static, the parameter of the resulting function is ignored.
        /// </summary>
        /// <typeparam name="TDeclaringType">
        /// The type of the parameter of the resulting function. This type must be compatible
        /// with the <see cref="MemberInfo.DeclaringType"/> of the <paramref name="field"/>
        /// parameter.
        /// </typeparam>
        /// <typeparam name="TFieldType">
        /// The return type of the resulting function. This type must be compatible with the
        /// <see cref="FieldInfo.FieldType"/> of the <paramref name="field"/> parameter.
        /// </typeparam>
        /// <param name="field">The field to create the getter function for.</param>
        /// <returns>A function that gets the field value.</returns>
        public static Func<TDeclaringType, TFieldType> CreateGetter<TDeclaringType, TFieldType>(this FieldInfo field)
        {
            if (field is null)
                throw new ArgumentNullException(nameof(field));
            if (!field.DeclaringType.IsAssignableFrom(typeof(TDeclaringType)))
                throw new ArgumentException("field.DeclaringType must be assignable from TDeclaringType", nameof(field));
            if (!typeof(TFieldType).IsAssignableFrom(field.FieldType))
                throw new ArgumentException("TFieldType must be assignable from field.FieldType", nameof(field));

            var getter = new FieldGetter<TDeclaringType, TFieldType>(field);
            QueueUserWorkItem(getter, g => g.SetOptimizedFunc());
            return getter.GetValue;
        }

        /// <summary>
        /// Creates an action that sets the value of the specified field. The first parameter
        /// of the resulting function takes the object whose field value is to be set. If the
        /// specified field is static, the first parameter of the resulting function is ignored.
        /// The second parameter of the resulting function takes the new value of the field.
        /// </summary>
        /// <param name="field">The field to create the setter action for.</param>
        /// <returns>An action that sets the field value.</returns>
        public static Action<object, object> CreateSetter(this FieldInfo field)
        {
            if (field is null)
                throw new ArgumentNullException(nameof(field));
            if (field.IsInitOnly)
                throw new ArgumentException("field cannot be readonly", nameof(field));

            var setter = new FieldSetter(field);
            QueueUserWorkItem(setter, s => s.SetOptimizedAction());
            return setter.SetValue;
        }

        /// <summary>
        /// Creates an action that sets the value of the specified field. The first parameter
        /// of the resulting function takes the object whose field value is to be set. If the
        /// specified field is static, the first parameter of the resulting function is ignored.
        /// The second parameter of the resulting function takes the new value of the field.
        /// </summary>
        /// <typeparam name="TFieldType">
        /// The type of the second parameter of the resulting action. This type must be compatible with
        /// the <see cref="FieldInfo.FieldType"/> of the <paramref name="field"/> parameter.
        /// </typeparam>
        /// <param name="field">The field to create the setter action for.</param>
        /// <returns>An action that sets the field value.</returns>
        public static Action<object, TFieldType> CreateSetter<TFieldType>(this FieldInfo field)
        {
            if (field is null)
                throw new ArgumentNullException(nameof(field));
            if (!field.FieldType.IsAssignableFrom(typeof(TFieldType)))
                throw new ArgumentException("field.FieldType must be assignable from TFieldType", nameof(field));
            if (field.IsInitOnly)
                throw new ArgumentException("field cannot be readonly", nameof(field));

            var setter = new FieldSetter<TFieldType>(field);
            QueueUserWorkItem(setter, s => s.SetOptimizedAction());
            return setter.SetValue;
        }

        /// <summary>
        /// Creates an action that sets the value of the specified field. The first parameter
        /// of the resulting function takes the object whose field value is to be set. If the
        /// specified field is static, the first parameter of the resulting function is ignored.
        /// The second parameter of the resulting function takes the new value of the field.
        /// </summary>
        /// <typeparam name="TDeclaringType">
        /// The type of the first parameter of the resulting function. This type must be compatible
        /// with the <see cref="MemberInfo.DeclaringType"/> of the <paramref name="field"/>
        /// parameter.
        /// </typeparam>
        /// <typeparam name="TFieldType">
        /// The type of the second parameter of the resulting action. This type must be compatible with
        /// the <see cref="FieldInfo.FieldType"/> of the <paramref name="field"/> parameter.
        /// </typeparam>
        /// <param name="field">The field to create the setter action for.</param>
        /// <returns>An action that sets the field value.</returns>
        public static Action<TDeclaringType, TFieldType> CreateSetter<TDeclaringType, TFieldType>(this FieldInfo field)
        {
            if (field is null)
                throw new ArgumentNullException(nameof(field));
            if (!field.FieldType.IsAssignableFrom(typeof(TFieldType)))
                throw new ArgumentException("field.FieldType must be assignable from TFieldType", nameof(field));
            if (!field.DeclaringType.IsAssignableFrom(typeof(TDeclaringType)))
                throw new ArgumentException("field.DeclaringType must be assignable from TDeclaringType", nameof(field));
            if (field.IsInitOnly)
                throw new ArgumentException("field cannot be readonly", nameof(field));

            var setter = new FieldSetter<TDeclaringType, TFieldType>(field);
            QueueUserWorkItem(setter, s => s.SetOptimizedAction());
            return setter.SetValue;
        }

        /// <summary>
        /// Creates a function that gets the value of the specified static field.
        /// </summary>
        /// <param name="field">The static field to create the getter function for.</param>
        /// <returns>A function that gets the static field value.</returns>
        public static Func<object> CreateStaticGetter(this FieldInfo field)
        {
            if (field is null)
                throw new ArgumentNullException(nameof(field));
            if (!field.IsStatic)
                throw new ArgumentException("Field must be static.", nameof(field));

            var getter = new StaticFieldGetter(field);
            QueueUserWorkItem(getter, g => g.SetOptimizedFunc());
            return getter.GetValue;
        }

        /// <summary>
        /// Creates a function that gets the value of the specified static field.
        /// </summary>
        /// <typeparam name="TFieldType">
        /// The return type of the resulting function. This type must be compatible with the
        /// <see cref="FieldInfo.FieldType"/> of the <paramref name="field"/> parameter.
        /// </typeparam>
        /// <param name="field">The static field to create the getter function for.</param>
        /// <returns>A function that gets the static field value.</returns>
        public static Func<TFieldType> CreateStaticGetter<TFieldType>(this FieldInfo field)
        {
            if (field is null)
                throw new ArgumentNullException(nameof(field));
            if (!typeof(TFieldType).IsAssignableFrom(field.FieldType))
                throw new ArgumentException("TFieldType must be assignable from field.FieldType", nameof(field));
            if (!field.IsStatic)
                throw new ArgumentException("Field must be static.", nameof(field));

            var getter = new StaticFieldGetter<TFieldType>(field);
            QueueUserWorkItem(getter, g => g.SetOptimizedFunc());
            return getter.GetValue;
        }

        /// <summary>
        /// Creates an action that sets the value of the specified static field. The
        /// parameter of the resulting function takes the new value of the static field.
        /// </summary>
        /// <param name="field">The static field to create the setter action for.</param>
        /// <returns>An action that sets the static field value.</returns>
        public static Action<object> CreateStaticSetter(this FieldInfo field)
        {
            if (field is null)
                throw new ArgumentNullException(nameof(field));
            if (field.IsInitOnly)
                throw new ArgumentException("field cannot be readonly", nameof(field));
            if (!field.IsStatic)
                throw new ArgumentException("Field must be static.", nameof(field));

            var setter = new StaticFieldSetter(field);
            QueueUserWorkItem(setter, s => s.SetOptimizedAction());
            return setter.SetValue;
        }

        /// <summary>
        /// Creates an action that sets the value of the specified static field. The
        /// parameter of the resulting function takes the new value of the static field.
        /// </summary>
        /// <typeparam name="TFieldType">
        /// The type of the parameter of the resulting action. This type must be compatible with
        /// the <see cref="FieldInfo.FieldType"/> of the <paramref name="field"/> parameter.
        /// </typeparam>
        /// <param name="field">The static field to create the setter action for.</param>
        /// <returns>An action that sets the static field value.</returns>
        public static Action<TFieldType> CreateStaticSetter<TFieldType>(this FieldInfo field)
        {
            if (field is null)
                throw new ArgumentNullException(nameof(field));
            if (!field.FieldType.IsAssignableFrom(typeof(TFieldType)))
                throw new ArgumentException("field.FieldType must be assignable from TFieldType", nameof(field));
            if (field.IsInitOnly)
                throw new ArgumentException("field cannot be readonly", nameof(field));
            if (!field.IsStatic)
                throw new ArgumentException("Field must be static.", nameof(field));

            var setter = new StaticFieldSetter<TFieldType>(field);
            QueueUserWorkItem(setter, s => s.SetOptimizedAction());
            return setter.SetValue;
        }

        #region Members for testing

        private static void QueueUserWorkItem<TState>(TState state, Action<TState> callback) =>
            _queueUserWorkItem.Invoke(s => callback((TState)s), state);

        internal static void SetQueueUserWorkItemAction(Action<WaitCallback, object> queueUserWorkItemAction) =>
            _queueUserWorkItem = queueUserWorkItemAction ?? ThreadPoolQueueUserWorkItem;

        private static Action<WaitCallback, object> _queueUserWorkItem = ThreadPoolQueueUserWorkItem;

        private static void ThreadPoolQueueUserWorkItem(WaitCallback callback, object state) =>
            ThreadPool.QueueUserWorkItem(callback, state);

        #endregion
    }
}
