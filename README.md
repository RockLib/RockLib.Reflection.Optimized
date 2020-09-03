# RockLib.Reflection.Optimized [![Build status](https://ci.appveyor.com/api/projects/status/p3ovl21n6hoih61f?svg=true)](https://ci.appveyor.com/project/RockLib/rocklib-reflection-optimized) [![NuGet](https://img.shields.io/nuget/vpre/RockLib.Reflection.Optimized.svg)](https://www.nuget.org/packages/RockLib.Reflection.Optimized)

*Extension methods to improve reflection performance.*

##### Table of Contents
- [Installing as Git submodule / shared project](#installing-as-git-submodule--shared-project)
- [PropertyInfo and FieldInfo extension methods](#propertyinfo-and-fieldinfo-extension-methods)
  - [CreateGetter / CreateSetter](#creategetter--createsetter)
    - [Overloads](#overloads)
    - [Static properties](#static-properties)
  - [CreateStaticGetter / CreateStaticSetter](#createstaticgetter--createstaticsetter)
    - [Overloads](#overloads-1)
- [Undecorate extension method](#undecorate-extension-method)

------

## Installing as Git submodule / shared project

Besides as a nuget package, another way to consume this library is as a submodule to an existing git repository. Add the RockLib.Reflection.Optimized submodule to your git repository. Then, add the shared project found in the submodule at `/RockLib.Reflection.Optimized.Shared/RockLib.Reflection.Optimized.Shared.shproj` to your solution. Finally, add a shared project reference to the project that needs optimized reflection.

Consuming this library in this manner is intended for use by other libraries - it eliminates a nuget dependency and the optimized reflection extension methods are not exposed publicly.

## PropertyInfo and FieldInfo extension methods

RockLib.Reflection.Optimized has extension methods for two types: [`System.Reflection.PropertyInfo`](https://msdn.microsoft.com/en-us/library/system.reflection.propertyinfo.aspx) and [`System.Reflection.FieldInfo`](https://msdn.microsoft.com/en-us/library/system.reflection.fieldinfo.aspx). These extension methods create functions at runtime that get or set the specified property or field.

### CreateGetter / CreateSetter

The following code snippet demonstrates usage of the `CreateGetter` and `CreateSetter` extension methods for `PropertyInfo` (usage for `FieldInfo` is identical):

```c#
using System.Reflection;
using RockLib.Reflection.Optimized;

public class Foo
{
    public int Bar { get; set; }
}

void Main()
{
    PropertyInfo property = typeof(Foo).GetProperty("Bar");
    
    Action<object, object> setBar = property.CreateSetter();
    Func<object, object> getBar = property.CreateGetter();

    Foo foo = new Foo();

    setBar(foo, 123); // Sets the value of the Bar property
    int bar = getBar(foo); // Gets the value of the Bar property
}
```

#### Overloads

The `CreateGetter` and `CreateSetter` extension methods for `PropertyInfo` and `FieldInfo` each have three overloads, allowing the parameters of the resulting delegates to be customized.

| Overload  | Return Type |
| --- | --- |
| `CreateGetter` | `Func<object, object>` |
| `CreateSetter` | `Action<object, object>` |
| `CreateGetter<TPropertyType>` | `Func<object, TPropertyType>` |
| `CreateSetter<TPropertyType>` | `Action<object, TPropertyType>` |
| `CreateGetter<TDeclaringType, TPropertyType>` | `Func<TDeclaringType, TPropertyType>` |
| `CreateSetter<TDeclaringType, TPropertyType>` | `Action<TDeclaringType, TPropertyType>` |

#### Static properties

If a `PropertyInfo` or `FieldInfo` represents a static property or field, then the first parameter of the functions returned by the `CreateGetter` and `CreateSetter` methods are ignored. When invokine functions that access static properties or fields, the caller can safely pass `null` for the first parameter.

### CreateStaticGetter / CreateStaticSetter

If it is known that a `PropertyInfo` or `FieldInfo` is static, then the `CreateStaticGetter` and `CreateStaticSetter` extension methods can be used, as in the following `FieldInfo` example (usage for `PropertyInfo` is identical):

```c#
using System.Reflection;
using RockLib.Reflection.Optimized;

public static class Foo
{
    public static int Bar;
}

void Main()
{
    FieldInfo field = typeof(Foo).GetField("Bar");
    
    Action<object> setBar = field.CreateStaticSetter();
    Func<object> getBar = field.CreateStaticGetter();

    Foo foo = new Foo();

    setBar(123); // Sets the value of the Foo.Bar property
    int bar = getBar(); // Gets the value of the Foo.Bar property
}
```

#### Overloads

Similar to their non-static counterpoints, the `CreateStaticGetter` and `CreateStaticSetter` extension methods for `PropertyInfo` and `FieldInfo` each have two overloads, allowing the parameters of the resulting delegates to be customized.

| Overload  | Return Type |
| --- | --- |
| `CreateStaticGetter` | `Func<object>` |
| `CreateStaticSetter` | `Action<object>` |
| `CreateStaticGetter<TPropertyType>` | `Func<TPropertyType>` |
| `CreateStaticSetter<TPropertyType>` | `Action<TPropertyType>` |

## Undecorate extension method

RockLib.Reflection.Optimized also contains an `Undecorate` extension method. This extension method checks to see if an object implementing an interface is a decorator for that interface, and if it is, unwraps it. __An object is considered a decorator if it has an instance field of the same type as the interface it implements.__ To unwrap a decorator object, the value of its interface instance field is used instead. The following example demonstrates usage of the `Undecorate` extension method:

```c#
void Main()
{
    IFoo foo = new FooDecorator(new AnotherFooDecorator { Foo = new MutableFoo() });
    
    if (foo.Undecorate() is MutableFoo mutableFoo)
        mutableFoo.Bar = 123;
}

public interface IFoo
{
    int Bar { get; }
}

public class MutableFoo : IFoo
{
    public int Bar { get; set; }
}

public class FooDecorator : IFoo
{
    private readonly IFoo _foo;    
    public FooDecorator(IFoo foo) => _foo = foo;
    public int Bar => _foo.Bar;
}

public class AnotherFooDecorator : IFoo
{
    public IFoo Foo { get; set; }
    public int Bar => Foo?.Bar ?? 0;
}
```
