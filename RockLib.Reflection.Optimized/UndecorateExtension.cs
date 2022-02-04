using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RockLib.Reflection.Optimized
{
    using UndecorateFunc = Func<object, object>;
    using static BindingFlags;

    /// <summary>
    /// Defines extension methods for undecorating objects.
    /// </summary>
    public static class UndecorateExtension
    {
        private static readonly ConcurrentDictionary<Tuple<Type, Type>, UndecorateFunc> _undecorateFuncs = new ConcurrentDictionary<Tuple<Type, Type>, UndecorateFunc>();

        /// <summary>
        /// Undecorates the specified object if it is a decorator implementation of interface type <typeparamref
        /// name="T"/>, otherwise just returns the object.
        /// <para>
        /// A class is considered to be a decorator if it implements interface <typeparamref name=
        /// "T"/> and has an instance field of type <typeparamref name="T"/>. To undecorate such an
        /// object, the value of its instance field is used.
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
                if (GetInterfaceField(concreteType, interfaceType) is FieldInfo field)
                    return field.CreateGetter();

                // Returning a null function indicates that concreteType is not a decorator class.
                return null;
            });

        private static FieldInfo GetInterfaceField(Type concreteType, Type interfaceType) =>
            GetAllFields(concreteType).FirstOrDefault(f => f.FieldType == interfaceType);

        private static IEnumerable<FieldInfo> GetAllFields(Type type)
        {
            if (type is null || type.BaseType is null)
                return Enumerable.Empty<FieldInfo>();

            return type.GetFields(Public | NonPublic | Instance | DeclaredOnly).Concat(GetAllFields(type.BaseType));
        }
    }
}
