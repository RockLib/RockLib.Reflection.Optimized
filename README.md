# RockLib.Reflection.Optimized

*Extension methods to improve reflection performance.*

## Installation

### Nuget

```powershell
PM> Install-Package RockLib.Reflection.Optimized
```

### Git submodule / shared project

Or add this repository as a submodule to an existing git project. Then add the shared project found at `` to the solution. Finally, add a shared project reference to the project that needs optimized reflection. Consuming this project in this manner is intended for use by other libraries - it eliminates a nuget dependency and the optimized reflection extension methods are not exposed publicly.

## Quick start

Start with a [`System.Reflection.PropertyInfo`](https://msdn.microsoft.com/en-us/library/system.reflection.propertyinfo.aspx). Then call the `CreateGetter` or `CreateSetter` extension method from it.

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

## Overloads

The `CreateGetter` and `CreateSetter` extension methods each have three overloads, allowing the parameters of the resulting delegates to be customized.

| Overload  | Return Type |
| --- | --- |
| `CreateGetter` | `Func<object, object>` |
| `CreateSetter` | `Action<object, object>` |
| `CreateGetter<TPropertyType>` | `Func<object, TPropertyType>` |
| `CreateSetter<TPropertyType>` | `Action<object, TPropertyType>` |
| `CreateGetter<TDeclaringType, TPropertyType>` | `Func<TDeclaringType, TPropertyType>` |
| `CreateSetter<TDeclaringType, TPropertyType>` | `Action<TDeclaringType, TPropertyType>` |
