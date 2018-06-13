using FluentAssertions;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Xunit;

namespace RockLib.Reflection.Optimized.Tests
{
    public class PropertySetterTests
    {
        private static readonly PropertyInfo _property = typeof(Foo).GetProperty(nameof(Foo.Bar));

        [Fact]
        public void PropertySetterWorks()
        {
            var foo = new Foo();

            var setter = new PropertySetter(_property);

            setter.Action.Target.Should().BeSameAs(_property);
            setter.Action.Method.Name.Should().Be(nameof(_property.SetValue));
            setter.Action.Method.DeclaringType.Should().Be(typeof(PropertyInfo));

            var reflectionTimer = Stopwatch.StartNew();
            setter.SetValue(foo, 123);
            reflectionTimer.Stop();

            foo.Bar.Should().Be(123);

            setter.SetOptimizedAction();

            setter.Action.Target.Should().NotBeSameAs(_property);
            setter.Action.Method.Name.Should().Be(PropertySetter.SetValueOptimized);
            setter.Action.Method.DeclaringType.Should().NotBe(typeof(PropertyInfo));

            var optimizedTimer = Stopwatch.StartNew();
            setter.SetValue(foo, 456);
            optimizedTimer.Stop();

            foo.Bar.Should().Be(456);

            optimizedTimer.Elapsed.Should().BeLessThan(reflectionTimer.Elapsed);
        }

        [Fact]
        public void PropertySetter1Works()
        {
            var foo = new Foo();

            var setter = new PropertySetter<int>(_property);

            setter.Action.Target.Should().BeSameAs(setter);
            setter.Action.Method.Name.Should().Be(nameof(setter.SetValueReflection));
            setter.Action.Method.DeclaringType.Should().Be(typeof(PropertySetter<int>));

            var reflectionTimer = Stopwatch.StartNew();
            setter.SetValue(foo, 123);
            reflectionTimer.Stop();

            foo.Bar.Should().Be(123);

            setter.SetOptimizedAction();

            setter.Action.Target.Should().NotBeSameAs(setter);
            setter.Action.Method.Name.Should().Be(PropertySetter.SetValueOptimized);
            setter.Action.Method.DeclaringType.Should().NotBe(typeof(PropertySetter<int>));

            var optimizedTimer = Stopwatch.StartNew();
            setter.SetValue(foo, 456);
            optimizedTimer.Stop();

            foo.Bar.Should().Be(456);

            optimizedTimer.Elapsed.Should().BeLessThan(reflectionTimer.Elapsed);
        }

        [Fact]
        public void PropertySetter2Works()
        {
            var foo = new Foo();

            var setter = new PropertySetter<Foo, int>(_property);

            setter.Action.Target.Should().BeSameAs(setter);
            setter.Action.Method.Name.Should().Be(nameof(setter.SetValueReflection));
            setter.Action.Method.DeclaringType.Should().Be(typeof(PropertySetter<Foo, int>));

            var reflectionTimer = Stopwatch.StartNew();
            setter.SetValue(foo, 123);
            reflectionTimer.Stop();

            foo.Bar.Should().Be(123);

            setter.SetOptimizedAction();

            setter.Action.Target.Should().NotBeSameAs(setter);
            setter.Action.Method.Name.Should().Be(PropertySetter.SetValueOptimized);
            setter.Action.Method.DeclaringType.Should().NotBe(typeof(PropertySetter<Foo, int>));

            var optimizedTimer = Stopwatch.StartNew();
            setter.SetValue(foo, 456);
            optimizedTimer.Stop();

            foo.Bar.Should().Be(456);

            optimizedTimer.Elapsed.Should().BeLessThan(reflectionTimer.Elapsed);
        }

        public class Foo
        {
            public int Bar { get; set; }
        }
    }
}
