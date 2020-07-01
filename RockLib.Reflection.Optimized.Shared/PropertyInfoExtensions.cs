using System;
using System.Reflection;
using System.Threading;

namespace RockLib.Reflection.Optimized
{
    /// <summary>
    /// Defines extension methods that return <see cref="Func{T, TResult}"/> or <see cref="Action{T1, T2}"/> delegates
    /// that get or set a property value of an object. These delegates provide much faster access than using reflection.
    /// </summary>
#if ROCKLIB_REFLECTION_OPTIMIZED
    public static class PropertyInfoExtensions
#else
    internal static partial class PropertyInfoExtensions
#endif
    {
        /// <summary>
        /// Creates a function that gets the value of the specified property. The parameter of the
        /// resulting function takes the object whose property value is to be accessed. If the
        /// specified property is static, the parameter of the resulting function is ignored.
        /// </summary>
        /// <param name="property">The property to create the getter function for.</param>
        /// <returns>A function that gets the property value.</returns>
        public static Func<object, object> CreateGetter(this PropertyInfo property)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));
            if (!property.CanRead || !property.GetMethod.IsPublic)
                throw new ArgumentException("Property must have public getter.", nameof(property));

            var getter = new PropertyGetter(property);
            QueueUserWorkItem(getter, g => g.SetOptimizedFunc());
            return getter.GetValue;
        }

        /// <summary>
        /// Creates a function that gets the value of the specified property. The parameter of the
        /// resulting function takes the object whose property value is to be accessed. If the
        /// specified property is static, the parameter of the resulting function is ignored.
        /// </summary>
        /// <typeparam name="TPropertyType">
        /// The return type of the resulting function. This type must be compatible with the
        /// <see cref="PropertyInfo.PropertyType"/> of the <paramref name="property"/> parameter.
        /// </typeparam>
        /// <param name="property">The property to create the getter function for.</param>
        /// <returns>A function that gets the property value.</returns>
        public static Func<object, TPropertyType> CreateGetter<TPropertyType>(this PropertyInfo property)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));
            if (!typeof(TPropertyType).IsAssignableFrom(property.PropertyType))
                throw new ArgumentException("TPropertyType must be assignable from property.PropertyType", nameof(property));
            if (!property.CanRead || !property.GetMethod.IsPublic)
                throw new ArgumentException("property must have public getter", nameof(property));

            var getter = new PropertyGetter<TPropertyType>(property);
            QueueUserWorkItem(getter, g => g.SetOptimizedFunc());
            return getter.GetValue;
        }

        /// <summary>
        /// Creates a function that gets the value of the specified property. The parameter of the
        /// resulting function takes the object whose property value is to be accessed. If the
        /// specified property is static, the parameter of the resulting function is ignored.
        /// </summary>
        /// <typeparam name="TDeclaringType">
        /// The type of the parameter of the resulting function. This type must be compatible
        /// with the <see cref="MemberInfo.DeclaringType"/> of the <paramref name="property"/>
        /// parameter.
        /// </typeparam>
        /// <typeparam name="TPropertyType">
        /// The return type of the resulting function. This type must be compatible with the
        /// <see cref="PropertyInfo.PropertyType"/> of the <paramref name="property"/> parameter.
        /// </typeparam>
        /// <param name="property">The property to create the getter function for.</param>
        /// <returns>A function that gets the property value.</returns>
        public static Func<TDeclaringType, TPropertyType> CreateGetter<TDeclaringType, TPropertyType>(this PropertyInfo property)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));
            if (!property.DeclaringType.IsAssignableFrom(typeof(TDeclaringType)))
                throw new ArgumentException("property.DeclaringType must be assignable from TDeclaringType", nameof(property));
            if (!typeof(TPropertyType).IsAssignableFrom(property.PropertyType))
                throw new ArgumentException("TPropertyType must be assignable from property.PropertyType", nameof(property));
            if (!property.CanRead || !property.GetMethod.IsPublic)
                throw new ArgumentException("property must have public getter", nameof(property));

            var getter = new PropertyGetter<TDeclaringType, TPropertyType>(property);
            QueueUserWorkItem(getter, g => g.SetOptimizedFunc());
            return getter.GetValue;
        }

        /// <summary>
        /// Creates an action that sets the value of the specified property. The first parameter
        /// of the resulting function takes the object whose property value is to be set. If the
        /// specified property is static, the first parameter of the resulting function is ignored.
        /// The second parameter of the resulting function takes the new value of the property.
        /// </summary>
        /// <param name="property">The property to create the setter action for.</param>
        /// <returns>An action that sets the property value.</returns>
        public static Action<object, object> CreateSetter(this PropertyInfo property)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));
            if (!property.CanWrite || !property.SetMethod.IsPublic)
                throw new ArgumentException("property must have public setter", nameof(property));

            var setter = new PropertySetter(property);
            QueueUserWorkItem(setter, s => s.SetOptimizedAction());
            return setter.SetValue;
        }

        /// <summary>
        /// Creates an action that sets the value of the specified property. The first parameter
        /// of the resulting function takes the object whose property value is to be set. If the
        /// specified property is static, the first parameter of the resulting function is ignored.
        /// The second parameter of the resulting function takes the new value of the property.
        /// </summary>
        /// <typeparam name="TPropertyType">
        /// The type of the second parameter of the resulting action. This type must be compatible with
        /// the <see cref="PropertyInfo.PropertyType"/> of the <paramref name="property"/> parameter.
        /// </typeparam>
        /// <param name="property">The property to create the setter action for.</param>
        /// <returns>An action that sets the property value.</returns>
        public static Action<object, TPropertyType> CreateSetter<TPropertyType>(this PropertyInfo property)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));
            if (!property.PropertyType.IsAssignableFrom(typeof(TPropertyType)))
                throw new ArgumentException("property.PropertyType must be assignable from TPropertyType", nameof(property));
            if (!property.CanWrite || !property.SetMethod.IsPublic)
                throw new ArgumentException("property must have public setter", nameof(property));

            var setter = new PropertySetter<TPropertyType>(property);
            QueueUserWorkItem(setter, s => s.SetOptimizedAction());
            return setter.SetValue;
        }

        /// <summary>
        /// Creates an action that sets the value of the specified property. The first parameter
        /// of the resulting function takes the object whose property value is to be set. If the
        /// specified property is static, the first parameter of the resulting function is ignored.
        /// The second parameter of the resulting function takes the new value of the property.
        /// </summary>
        /// <typeparam name="TDeclaringType">
        /// The type of the first parameter of the resulting function. This type must be compatible
        /// with the <see cref="MemberInfo.DeclaringType"/> of the <paramref name="property"/>
        /// parameter.
        /// </typeparam>
        /// <typeparam name="TPropertyType">
        /// The type of the second parameter of the resulting action. This type must be compatible with
        /// the <see cref="PropertyInfo.PropertyType"/> of the <paramref name="property"/> parameter.
        /// </typeparam>
        /// <param name="property">The property to create the setter action for.</param>
        /// <returns>An action that sets the property value.</returns>
        public static Action<TDeclaringType, TPropertyType> CreateSetter<TDeclaringType, TPropertyType>(this PropertyInfo property)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));
            if (!property.PropertyType.IsAssignableFrom(typeof(TPropertyType)))
                throw new ArgumentException("property.PropertyType must be assignable from TPropertyType", nameof(property));
            if (!property.DeclaringType.IsAssignableFrom(typeof(TDeclaringType)))
                throw new ArgumentException("property.DeclaringType must be assignable from TDeclaringType", nameof(property));
            if (!property.CanWrite || !property.SetMethod.IsPublic)
                throw new ArgumentException("property must have public setter", nameof(property));

            var setter = new PropertySetter<TDeclaringType, TPropertyType>(property);
            QueueUserWorkItem(setter, s => s.SetOptimizedAction());
            return setter.SetValue;
        }

        /// <summary>
        /// Creates a function that gets the value of the specified static property.
        /// </summary>
        /// <param name="property">The static property to create the getter function for.</param>
        /// <returns>A function that gets the static property value.</returns>
        public static Func<object> CreateStaticGetter(this PropertyInfo property)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));
            if (!property.CanRead || !property.GetMethod.IsPublic)
                throw new ArgumentException("Property must have public getter.", nameof(property));
            if (!property.GetMethod.IsStatic)
                throw new ArgumentException("Property must be static.", nameof(property));

            var getter = new StaticPropertyGetter(property);
            QueueUserWorkItem(getter, g => g.SetOptimizedFunc());
            return getter.GetValue;
        }

        /// <summary>
        /// Creates a function that gets the value of the specified static property.
        /// </summary>
        /// <typeparam name="TPropertyType">
        /// The return type of the resulting function. This type must be compatible with the
        /// <see cref="PropertyInfo.PropertyType"/> of the <paramref name="property"/> parameter.
        /// </typeparam>
        /// <param name="property">The static property to create the getter function for.</param>
        /// <returns>A function that gets the static property value.</returns>
        public static Func<TPropertyType> CreateStaticGetter<TPropertyType>(this PropertyInfo property)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));
            if (!typeof(TPropertyType).IsAssignableFrom(property.PropertyType))
                throw new ArgumentException("TPropertyType must be assignable from property.PropertyType", nameof(property));
            if (!property.CanRead || !property.GetMethod.IsPublic)
                throw new ArgumentException("property must have public getter", nameof(property));
            if (!property.GetMethod.IsStatic)
                throw new ArgumentException("Property must be static.", nameof(property));

            var getter = new StaticPropertyGetter<TPropertyType>(property);
            QueueUserWorkItem(getter, g => g.SetOptimizedFunc());
            return getter.GetValue;
        }

        /// <summary>
        /// Creates an action that sets the value of the specified static property. The
        /// parameter of the resulting function takes the new value of the static property.
        /// </summary>
        /// <param name="property">The static property to create the setter action for.</param>
        /// <returns>An action that sets the static property value.</returns>
        public static Action<object> CreateStaticSetter(this PropertyInfo property)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));
            if (!property.CanWrite || !property.SetMethod.IsPublic)
                throw new ArgumentException("property must have public setter", nameof(property));
            if (!property.SetMethod.IsStatic)
                throw new ArgumentException("Property must be static.", nameof(property));

            var setter = new StaticPropertySetter(property);
            QueueUserWorkItem(setter, s => s.SetOptimizedAction());
            return setter.SetValue;
        }

        /// <summary>
        /// Creates an action that sets the value of the specified static property. The
        /// parameter of the resulting function takes the new value of the static property.
        /// </summary>
        /// <typeparam name="TPropertyType">
        /// The type of the parameter of the resulting action. This type must be compatible with
        /// the <see cref="PropertyInfo.PropertyType"/> of the <paramref name="property"/> parameter.
        /// </typeparam>
        /// <param name="property">The static property to create the setter action for.</param>
        /// <returns>An action that sets the static property value.</returns>
        public static Action<TPropertyType> CreateStaticSetter<TPropertyType>(this PropertyInfo property)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));
            if (!property.PropertyType.IsAssignableFrom(typeof(TPropertyType)))
                throw new ArgumentException("property.PropertyType must be assignable from TPropertyType", nameof(property));
            if (!property.CanWrite || !property.SetMethod.IsPublic)
                throw new ArgumentException("property must have public setter", nameof(property));
            if (!property.SetMethod.IsStatic)
                throw new ArgumentException("Property must be static.", nameof(property));

            var setter = new StaticPropertySetter<TPropertyType>(property);
            QueueUserWorkItem(setter, s => s.SetOptimizedAction());
            return setter.SetValue;
        }

        #region Members for testing

        private static void QueueUserWorkItem<TState>(TState state, Action<TState> callback) =>
            _queueUserWorkItem.Invoke(s => callback((TState)s), state);

#if ROCKLIB_REFLECTION_OPTIMIZED
        internal static void SetQueueUserWorkItemAction(Action<WaitCallback, object> queueUserWorkItemAction) =>
            _queueUserWorkItem = queueUserWorkItemAction ?? ThreadPoolQueueUserWorkItem;
#endif

        private static Action<WaitCallback, object> _queueUserWorkItem = ThreadPoolQueueUserWorkItem;

        private static void ThreadPoolQueueUserWorkItem(WaitCallback callback, object state) =>
            ThreadPool.QueueUserWorkItem(callback, state);

        #endregion
    }
}
