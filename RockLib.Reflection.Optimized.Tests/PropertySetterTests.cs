using FluentAssertions;
using System;
using System.Diagnostics;
using System.Reflection;
using Xunit;

namespace RockLib.Reflection.Optimized.Tests
{
    public class PropertySetterTests
    {
        private static readonly PropertyInfo _property = typeof(Foo).GetProperty(nameof(Foo.Bar))!;

        [Fact]
        public void PropertySetterWorks()
        {
            var foo = new Foo();

            var setter = new PropertySetter(_property);

            setter.Action.Target.Should().BeSameAs(_property);
            setter.Action.Method.Name.Should().Be(nameof(_property.SetValue));
            setter.Action.Method.DeclaringType.Should().Be(typeof(PropertyInfo));

            using (var gc = new GCNoRegion(4194304))
            {
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
        }

        [Fact]
        public void PropertySetter1Works()
        {
            var foo = new Foo();

            var setter = new PropertySetter<int>(_property);

            setter.Action.Target.Should().BeSameAs(setter);
            setter.Action.Method.Name.Should().Be(nameof(setter.SetValueReflection));
            setter.Action.Method.DeclaringType.Should().Be(typeof(PropertySetter<int>));

            using (var gc = new GCNoRegion(4194304))
            {
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
        }

        [Fact]
        public void PropertySetter2Works()
        {
            var foo = new Foo();

            var setter = new PropertySetter<Foo, int>(_property);

            setter.Action.Target.Should().BeSameAs(setter);
            setter.Action.Method.Name.Should().Be(nameof(setter.SetValueReflection));
            setter.Action.Method.DeclaringType.Should().Be(typeof(PropertySetter<Foo, int>));

            using (var gc = new GCNoRegion(4194304))
            {
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
        }

        [Fact]
        public void PropertySetterWorksWithStatic()
        {
            Baz.Qux = -1;

            var property = typeof(Baz).GetProperty(nameof(Baz.Qux));

            var setter = new PropertySetter(property!);

            setter.Action.Target.Should().BeSameAs(property);
            setter.Action.Method.Name.Should().Be(nameof(property.SetValue));
            setter.Action.Method.DeclaringType.Should().Be(typeof(PropertyInfo));

            using (var gc = new GCNoRegion(4194304))
            {
                var reflectionTimer = Stopwatch.StartNew();
                setter.SetValue(null!, 123);
                reflectionTimer.Stop();

                Baz.Qux.Should().Be(123);

                setter.SetOptimizedAction();

                setter.Action.Target.Should().NotBeSameAs(property);
                setter.Action.Method.Name.Should().Be(PropertySetter.SetValueOptimized);
                setter.Action.Method.DeclaringType.Should().NotBe(typeof(PropertyInfo));

                var optimizedTimer = Stopwatch.StartNew();
                setter.SetValue(null!, 456);
                optimizedTimer.Stop();

                Baz.Qux.Should().Be(456);

                optimizedTimer.Elapsed.Should().BeLessThan(reflectionTimer.Elapsed);
            }
        }

        [Fact]
        public void PropertySetter1WorksWithStatic()
        {
            Baz.Qux = -1;

            var property = typeof(Baz).GetProperty(nameof(Baz.Qux));

            var setter = new PropertySetter<int>(property!);

            setter.Action.Target.Should().BeSameAs(setter);
            setter.Action.Method.Name.Should().Be(nameof(setter.SetValueReflection));
            setter.Action.Method.DeclaringType.Should().Be(typeof(PropertySetter<int>));

            using (var gc = new GCNoRegion(4194304))
            {
                var reflectionTimer = Stopwatch.StartNew();
                setter.SetValue(null!, 123);
                reflectionTimer.Stop();

                Baz.Qux.Should().Be(123);

                setter.SetOptimizedAction();

                setter.Action.Target.Should().NotBeSameAs(setter);
                setter.Action.Method.Name.Should().Be(PropertySetter.SetValueOptimized);
                setter.Action.Method.DeclaringType.Should().NotBe(typeof(PropertySetter<int>));

                var optimizedTimer = Stopwatch.StartNew();
                setter.SetValue(null!, 456);
                optimizedTimer.Stop();

                Baz.Qux.Should().Be(456);

                optimizedTimer.Elapsed.Should().BeLessThan(reflectionTimer.Elapsed);
            }
        }

        [Fact]
        public void PropertySetter2WorksWithStatic()
        {
            Baz.Qux = -1;

            var property = typeof(Baz).GetProperty(nameof(Baz.Qux));

            var setter = new PropertySetter<Foo, int>(property!);

            setter.Action.Target.Should().BeSameAs(setter);
            setter.Action.Method.Name.Should().Be(nameof(setter.SetValueReflection));
            setter.Action.Method.DeclaringType.Should().Be(typeof(PropertySetter<Foo, int>));

            using (var gc = new GCNoRegion(4194304))
            {
                var reflectionTimer = Stopwatch.StartNew();
                setter.SetValue(null!, 123);
                reflectionTimer.Stop();

                Baz.Qux.Should().Be(123);

                setter.SetOptimizedAction();

                setter.Action.Target.Should().NotBeSameAs(setter);
                setter.Action.Method.Name.Should().Be(PropertySetter.SetValueOptimized);
                setter.Action.Method.DeclaringType.Should().NotBe(typeof(PropertySetter<Foo, int>));

                var optimizedTimer = Stopwatch.StartNew();
                setter.SetValue(null!, 456);
                optimizedTimer.Stop();

                Baz.Qux.Should().Be(456);

                optimizedTimer.Elapsed.Should().BeLessThan(reflectionTimer.Elapsed);
            }
        }

        [Fact]
        public void StaticPropertySetterWorks()
        {
            Baz.Qux = -1;

            var property = typeof(Baz).GetProperty(nameof(Baz.Qux));

            var setter = new StaticPropertySetter(property!);

            setter.Action.Target.Should().BeSameAs(setter);
            setter.Action.Method.Name.Should().Be(nameof(setter.SetValueReflection));
            setter.Action.Method.DeclaringType.Should().Be(typeof(StaticPropertySetter));

            using (var gc = new GCNoRegion(4194304))
            {
                var reflectionTimer = Stopwatch.StartNew();
                setter.SetValue(123);
                reflectionTimer.Stop();

                Baz.Qux.Should().Be(123);

                setter.SetOptimizedAction();

                setter.Action.Target.Should().NotBeSameAs(setter);
                setter.Action.Method.Name.Should().Be(StaticPropertySetter.SetStaticValueOptimized);
                setter.Action.Method.DeclaringType.Should().NotBe(typeof(StaticPropertySetter));

                var optimizedTimer = Stopwatch.StartNew();
                setter.SetValue(456);
                optimizedTimer.Stop();

                Baz.Qux.Should().Be(456);

                optimizedTimer.Elapsed.Should().BeLessThan(reflectionTimer.Elapsed);
            }
        }

        [Fact]
        public void StaticPropertySetter1Works()
        {
            Baz.Qux = -1;

            var property = typeof(Baz).GetProperty(nameof(Baz.Qux));

            var setter = new StaticPropertySetter<int>(property!);

            setter.Action.Target.Should().BeSameAs(setter);
            setter.Action.Method.Name.Should().Be(nameof(setter.SetValueReflection));
            setter.Action.Method.DeclaringType.Should().Be(typeof(StaticPropertySetter<int>));

            using (var gc = new GCNoRegion(4194304))
            {
                var reflectionTimer = Stopwatch.StartNew();
                setter.SetValue(123);
                reflectionTimer.Stop();

                Baz.Qux.Should().Be(123);

                setter.SetOptimizedAction();

                setter.Action.Target.Should().NotBeSameAs(setter);
                setter.Action.Method.Name.Should().Be(StaticPropertySetter.SetStaticValueOptimized);
                setter.Action.Method.DeclaringType.Should().NotBe(typeof(StaticPropertySetter<int>));

                var optimizedTimer = Stopwatch.StartNew();
                setter.SetValue(456);
                optimizedTimer.Stop();

                Baz.Qux.Should().Be(456);

                optimizedTimer.Elapsed.Should().BeLessThan(reflectionTimer.Elapsed);
            }
        }

        private sealed class Foo
        {
            public int Bar { get; set; }
        }

        private static class Baz
        {
            public static int Qux { get; set; } = -1;
        }
    }
}
