using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace RockLib.Reflection.Optimized
{
    using UndecorateFunc = Func<object, object>;
    using static BindingFlags;

    /// <summary>
    /// Defines extension methods for undecorating objects.
    /// </summary>
#if ROCKLIB_REFLECTION_OPTIMIZED
    public static class UndecorateExtension
#else
    internal static partial class UndecorateExtension
#endif
    {
        private static readonly ConcurrentDictionary<Tuple<Type, Type>, UndecorateFunc> _undecorateFuncs = new ConcurrentDictionary<Tuple<Type, Type>, UndecorateFunc>();

        /// <summary>
        /// Undecorates the specified object if it is a decorator implementation of interface type <typeparamref
        /// name="T"/>, otherwise just returns the object.
        /// <para>
        /// An object is considered to be decorated if it has an instance field of type
        /// <typeparamref name="T"/> and either a public constructor with a parameter of type
        /// <typeparamref name="T"/> or a public property of type <typeparamref name="T"/> that has
        /// a public setter. To undecorate such an object, the value of its instance field is used.
        /// </para>
        /// </summary>
        /// <typeparam name="T">The interface type to undecorate.</typeparam>
        /// <param name="value">The object to undecorate.</param>
        /// <returns>
        /// The undecorated value if <paramref name="value"/> is a decorator object, otherwise the
        /// same value.
        /// </returns>
        /// <exception cref="InvalidOperationException">If <typeparamref name="T"/> is not an interface type.</exception>
        public static T Undecorate<T>(this T value)
            where T : class
        {
            if (!typeof(T).IsInterface)
                throw new InvalidOperationException($"Generic type argument {nameof(T)} must be an interface, but was: {typeof(T).FullName}");

            while (value != null && GetUndecorateFunc(value.GetType(), typeof(T)) is UndecorateFunc undecorate)
                value = (T)undecorate(value);

            return value;
        }

        private static UndecorateFunc GetUndecorateFunc(Type concreteType, Type interfaceType) =>
            _undecorateFuncs.GetOrAdd(Tuple.Create(concreteType, interfaceType), _ =>
            {
                if (GetInterfaceField(concreteType, interfaceType) is FieldInfo field
                    && (HasInterfaceConstructorParameter(concreteType, interfaceType)
                        || HasInterfacePropertySetter(concreteType, interfaceType)))
                {
                    return field.CreateGetter();
                }

                // Returning a null function indicates that concreteType is not a decorator class.
                return null;
            });

        private static FieldInfo GetInterfaceField(Type concreteType, Type interfaceType) =>
            concreteType.GetFields(Public | NonPublic | Instance)
                .FirstOrDefault(f => f.FieldType == interfaceType);

        private static bool HasInterfaceConstructorParameter(Type concreteType, Type interfaceType) =>
            concreteType.GetConstructors(Public | Instance)
                .SelectMany(c => c.GetParameters())
                .Any(p => p.ParameterType == interfaceType);

        private static bool HasInterfacePropertySetter(Type concreteType, Type interfaceType) =>
            concreteType.GetProperties(Public | Instance)
                .Any(p => p.PropertyType == interfaceType && p.CanWrite && p.SetMethod.IsPublic);
    }
}
