using FluentAssertions;
using System;
using System.Diagnostics;
using System.Reflection;
using Xunit;

namespace RockLib.Reflection.Optimized.Tests
{
    public class PropertyGetterTests
    {
        [Fact]
        public void PropertyGetterWorks()
        {
            var foo = new Foo("abc");
            var property = typeof(Foo).GetProperty(nameof(Foo.Bar));

            var getter = new PropertyGetter(property);

            getter.Func.Target.Should().BeSameAs(property);
            getter.Func.Method.Name.Should().Be(nameof(property.GetValue));
            getter.Func.Method.DeclaringType.Should().Be(typeof(PropertyInfo));

            using (var gc = new GCNoRegion(4194304))
            {
                var reflectionTimer = Stopwatch.StartNew();
                object reflectionValue = getter.GetValue(foo);
                reflectionTimer.Stop();

                getter.SetOptimizedFunc();

                getter.Func.Target.Should().NotBeSameAs(property);
                getter.Func.Method.Name.Should().Be(PropertyGetter.GetValueOptimized);
                getter.Func.Method.DeclaringType.Should().NotBe(typeof(PropertyInfo));

                var optimizedTimer = Stopwatch.StartNew();
                object optimizedValue = getter.GetValue(foo);
                optimizedTimer.Stop();

                optimizedValue.Should().Be(reflectionValue);
                optimizedTimer.Elapsed.Should().BeLessThan(reflectionTimer.Elapsed);
            }
        }

        [Fact]
        public void PropertyGetter1Works()
        {
            var foo = new Foo("abc");
            var property = typeof(Foo).GetProperty(nameof(Foo.Bar));

            var getter = new PropertyGetter<string>(property);

            getter.Func.Target.Should().BeSameAs(getter);
            getter.Func.Method.Name.Should().Be(nameof(getter.GetValueReflection));
            getter.Func.Method.DeclaringType.Should().Be(typeof(PropertyGetter<string>));

            using (var gc = new GCNoRegion(4194304))
            {
                var reflectionTimer = Stopwatch.StartNew();
                string reflectionValue = getter.GetValue(foo);
                reflectionTimer.Stop();

                getter.SetOptimizedFunc();

                getter.Func.Target.Should().NotBeSameAs(getter);
                getter.Func.Method.Name.Should().Be(PropertyGetter.GetValueOptimized);
                getter.Func.Method.DeclaringType.Should().NotBe(typeof(PropertyGetter<string>));

                var optimizedTimer = Stopwatch.StartNew();
                string optimizedValue = getter.GetValue(foo);
                optimizedTimer.Stop();

                optimizedValue.Should().Be(reflectionValue);
                optimizedTimer.Elapsed.Should().BeLessThan(reflectionTimer.Elapsed);
            }
        }

        [Fact]
        public void PropertyGetter2Works()
        {
            var foo = new Foo("abc");
            var property = typeof(Foo).GetProperty(nameof(Foo.Bar));

            var getter = new PropertyGetter<Foo, string>(property);

            getter.Func.Target.Should().BeSameAs(getter);
            getter.Func.Method.Name.Should().Be(nameof(getter.GetValueReflection));
            getter.Func.Method.DeclaringType.Should().Be(typeof(PropertyGetter<Foo, string>));

            using (var gc = new GCNoRegion(4194304))
            {
                var reflectionTimer = Stopwatch.StartNew();
                string reflectionValue = getter.GetValue(foo);
                reflectionTimer.Stop();

                getter.SetOptimizedFunc();

                getter.Func.Target.Should().NotBeSameAs(getter);
                getter.Func.Method.Name.Should().Be(PropertyGetter.GetValueOptimized);
                getter.Func.Method.DeclaringType.Should().NotBe(typeof(PropertyGetter<Foo, string>));

                var optimizedTimer = Stopwatch.StartNew();
                string optimizedValue = getter.GetValue(foo);
                optimizedTimer.Stop();

                optimizedValue.Should().Be(reflectionValue);
                optimizedTimer.Elapsed.Should().BeLessThan(reflectionTimer.Elapsed);
            }
        }

        [Fact]
        public void PropertyGetterWorksWithBoxing()
        {
            var bar = new Bar { Baz = new Baz(123) };
            var property = typeof(Bar).GetProperty(nameof(Bar.Baz));

            var getter = new PropertyGetter(property);

            getter.Func.Target.Should().BeSameAs(property);
            getter.Func.Method.Name.Should().Be(nameof(property.GetValue));
            getter.Func.Method.DeclaringType.Should().Be(typeof(PropertyInfo));

            using (var gc = new GCNoRegion(4194304))
            {
                var reflectionTimer = Stopwatch.StartNew();
                object reflectionValue = getter.GetValue(bar);
                reflectionTimer.Stop();

                getter.SetOptimizedFunc();

                getter.Func.Target.Should().NotBeSameAs(property);
                getter.Func.Method.Name.Should().Be(PropertyGetter.GetValueOptimized);
                getter.Func.Method.DeclaringType.Should().NotBe(typeof(PropertyInfo));

                var optimizedTimer = Stopwatch.StartNew();
                object optimizedValue = getter.GetValue(bar);
                optimizedTimer.Stop();

                optimizedValue.Should().Be(reflectionValue);
                optimizedTimer.Elapsed.Should().BeLessThan(reflectionTimer.Elapsed);
            }
        }

        [Fact]
        public void PropertyGetter1WorksWithBoxing()
        {
            var bar = new Bar { Baz = new Baz(123) };
            var property = typeof(Bar).GetProperty(nameof(Bar.Baz));

            var getter = new PropertyGetter<IBaz>(property);

            getter.Func.Target.Should().BeSameAs(getter);
            getter.Func.Method.Name.Should().Be(nameof(getter.GetValueReflection));
            getter.Func.Method.DeclaringType.Should().Be(typeof(PropertyGetter<IBaz>));

            using (var gc = new GCNoRegion(4194304))
            {
                var reflectionTimer = Stopwatch.StartNew();
                IBaz reflectionValue = getter.GetValue(bar);
                reflectionTimer.Stop();

                getter.SetOptimizedFunc();

                getter.Func.Target.Should().NotBeSameAs(getter);
                getter.Func.Method.Name.Should().Be(PropertyGetter.GetValueOptimized);
                getter.Func.Method.DeclaringType.Should().NotBe(typeof(PropertyGetter<IBaz>));

                var optimizedTimer = Stopwatch.StartNew();
                IBaz optimizedValue = getter.GetValue(bar);
                optimizedTimer.Stop();

                optimizedValue.Should().Be(reflectionValue);
                optimizedTimer.Elapsed.Should().BeLessThan(reflectionTimer.Elapsed);
            }
        }

        [Fact]
        public void PropertyGetter2WorksWithBoxing()
        {
            var bar = new Bar { Baz = new Baz(123) };
            var property = typeof(Bar).GetProperty(nameof(Bar.Baz));

            var getter = new PropertyGetter<Bar, IBaz>(property);

            getter.Func.Target.Should().BeSameAs(getter);
            getter.Func.Method.Name.Should().Be(nameof(getter.GetValueReflection));
            getter.Func.Method.DeclaringType.Should().Be(typeof(PropertyGetter<Bar, IBaz>));

            using (var gc = new GCNoRegion(4194304))
            {
                var reflectionTimer = Stopwatch.StartNew();
                IBaz reflectionValue = getter.GetValue(bar);
                reflectionTimer.Stop();

                getter.SetOptimizedFunc();

                getter.Func.Target.Should().NotBeSameAs(getter);
                getter.Func.Method.Name.Should().Be(PropertyGetter.GetValueOptimized);
                getter.Func.Method.DeclaringType.Should().NotBe(typeof(PropertyGetter<Bar, IBaz>));

                var optimizedTimer = Stopwatch.StartNew();
                IBaz optimizedValue = getter.GetValue(bar);
                optimizedTimer.Stop();

                optimizedValue.Should().Be(reflectionValue);
                optimizedTimer.Elapsed.Should().BeLessThan(reflectionTimer.Elapsed);
            }
        }

        [Fact]
        public void PropertyGetterWorksWithStatic()
        {
            var property = typeof(Garply).GetProperty(nameof(Garply.Bar));

            var getter = new PropertyGetter(property);

            getter.Func.Target.Should().BeSameAs(property);
            getter.Func.Method.Name.Should().Be(nameof(property.GetValue));
            getter.Func.Method.DeclaringType.Should().Be(typeof(PropertyInfo));

            using (var gc = new GCNoRegion(4194304))
            {
                var reflectionTimer = Stopwatch.StartNew();
                object reflectionValue = getter.GetValue(null!);
                reflectionTimer.Stop();

                getter.SetOptimizedFunc();

                getter.Func.Target.Should().NotBeSameAs(property);
                getter.Func.Method.Name.Should().Be(PropertyGetter.GetValueOptimized);
                getter.Func.Method.DeclaringType.Should().NotBe(typeof(PropertyInfo));

                var optimizedTimer = Stopwatch.StartNew();
                object optimizedValue = getter.GetValue(null!);
                optimizedTimer.Stop();

                optimizedValue.Should().Be(reflectionValue);
                optimizedTimer.Elapsed.Should().BeLessThan(reflectionTimer.Elapsed);
            }
        }

        [Fact]
        public void PropertyGetter1WorksWithStatic()
        {
            var property = typeof(Garply).GetProperty(nameof(Garply.Bar));

            var getter = new PropertyGetter<string>(property);

            getter.Func.Target.Should().BeSameAs(getter);
            getter.Func.Method.Name.Should().Be(nameof(getter.GetValueReflection));
            getter.Func.Method.DeclaringType.Should().Be(typeof(PropertyGetter<string>));

            using (var gc = new GCNoRegion(4194304))
            {
                var reflectionTimer = Stopwatch.StartNew();
                string reflectionValue = getter.GetValue(null!);
                reflectionTimer.Stop();

                getter.SetOptimizedFunc();

                getter.Func.Target.Should().NotBeSameAs(getter);
                getter.Func.Method.Name.Should().Be(PropertyGetter.GetValueOptimized);
                getter.Func.Method.DeclaringType.Should().NotBe(typeof(PropertyGetter<string>));

                var optimizedTimer = Stopwatch.StartNew();
                string optimizedValue = getter.GetValue(null!);
                optimizedTimer.Stop();

                optimizedValue.Should().Be(reflectionValue);
                optimizedTimer.Elapsed.Should().BeLessThan(reflectionTimer.Elapsed);
            }
        }

        [Fact]
        public void PropertyGetter2WorksWithStatic()
        {
            var property = typeof(Garply).GetProperty(nameof(Garply.Bar));

            var getter = new PropertyGetter<Garply, string>(property);

            getter.Func.Target.Should().BeSameAs(getter);
            getter.Func.Method.Name.Should().Be(nameof(getter.GetValueReflection));
            getter.Func.Method.DeclaringType.Should().Be(typeof(PropertyGetter<Garply, string>));

            using (var gc = new GCNoRegion(4194304))
            {
                var reflectionTimer = Stopwatch.StartNew();
                string reflectionValue = getter.GetValue(null!);
                reflectionTimer.Stop();

                getter.SetOptimizedFunc();

                getter.Func.Target.Should().NotBeSameAs(getter);
                getter.Func.Method.Name.Should().Be(PropertyGetter.GetValueOptimized);
                getter.Func.Method.DeclaringType.Should().NotBe(typeof(PropertyGetter<Garply, string>));

                var optimizedTimer = Stopwatch.StartNew();
                string optimizedValue = getter.GetValue(null!);
                optimizedTimer.Stop();

                optimizedValue.Should().Be(reflectionValue);
                optimizedTimer.Elapsed.Should().BeLessThan(reflectionTimer.Elapsed);
            }
        }

        [Fact]
        public void StaticPropertyGetterWorks()
        {
            var property = typeof(Garply).GetProperty(nameof(Garply.Bar));

            var getter = new StaticPropertyGetter(property);

            getter.Func.Target.Should().BeSameAs(getter);
            getter.Func.Method.Name.Should().Be(nameof(getter.GetValueReflection));
            getter.Func.Method.DeclaringType.Should().Be(typeof(StaticPropertyGetter));

            using (var gc = new GCNoRegion(4194304))
            {
                var reflectionTimer = Stopwatch.StartNew();
                object reflectionValue = getter.GetValue();
                reflectionTimer.Stop();

                getter.SetOptimizedFunc();

                getter.Func.Target.Should().NotBeSameAs(getter);
                getter.Func.Method.Name.Should().Be(StaticPropertyGetter.GetStaticValueOptimized);
                getter.Func.Method.DeclaringType.Should().NotBe(typeof(StaticPropertyGetter));

                var optimizedTimer = Stopwatch.StartNew();
                object optimizedValue = getter.GetValue();
                optimizedTimer.Stop();

                optimizedValue.Should().Be(reflectionValue);
                optimizedTimer.Elapsed.Should().BeLessThan(reflectionTimer.Elapsed);
            }
        }

        [Fact]
        public void StaticPropertyGetter1Works()
        {
            var property = typeof(Garply).GetProperty(nameof(Garply.Bar));

            var getter = new StaticPropertyGetter<string>(property);

            getter.Func.Target.Should().BeSameAs(getter);
            getter.Func.Method.Name.Should().Be(nameof(getter.GetValueReflection));
            getter.Func.Method.DeclaringType.Should().Be(typeof(StaticPropertyGetter<string>));

            using (var gc = new GCNoRegion(4194304))
            {
                var reflectionTimer = Stopwatch.StartNew();
                string reflectionValue = getter.GetValue();
                reflectionTimer.Stop();

                getter.SetOptimizedFunc();

                getter.Func.Target.Should().NotBeSameAs(getter);
                getter.Func.Method.Name.Should().Be(StaticPropertyGetter.GetStaticValueOptimized);
                getter.Func.Method.DeclaringType.Should().NotBe(typeof(StaticPropertyGetter<string>));

                var optimizedTimer = Stopwatch.StartNew();
                string optimizedValue = getter.GetValue();
                optimizedTimer.Stop();

                optimizedValue.Should().Be(reflectionValue);
                optimizedTimer.Elapsed.Should().BeLessThan(reflectionTimer.Elapsed);
            }
        }

        [Fact]
        public void StaticPropertyGetterWorksWithBoxing()
        {
            var property = typeof(Garply).GetProperty(nameof(Garply.Baz));

            var getter = new StaticPropertyGetter(property);

            getter.Func.Target.Should().BeSameAs(getter);
            getter.Func.Method.Name.Should().Be(nameof(getter.GetValueReflection));
            getter.Func.Method.DeclaringType.Should().Be(typeof(StaticPropertyGetter));

            using (var gc = new GCNoRegion(4194304))
            {
                var reflectionTimer = Stopwatch.StartNew();
                object reflectionValue = getter.GetValue();
                reflectionTimer.Stop();

                getter.SetOptimizedFunc();

                getter.Func.Target.Should().NotBeSameAs(getter);
                getter.Func.Method.Name.Should().Be(StaticPropertyGetter.GetStaticValueOptimized);
                getter.Func.Method.DeclaringType.Should().NotBe(typeof(StaticPropertyGetter));

                var optimizedTimer = Stopwatch.StartNew();
                object optimizedValue = getter.GetValue();
                optimizedTimer.Stop();

                optimizedValue.Should().Be(reflectionValue);
                optimizedTimer.Elapsed.Should().BeLessThan(reflectionTimer.Elapsed);
            }
        }

        [Fact]
        public void StaticPropertyGetter1WorksWithBoxing()
        {
            var property = typeof(Garply).GetProperty(nameof(Garply.Baz));

            var getter = new StaticPropertyGetter<IBaz>(property);

            getter.Func.Target.Should().BeSameAs(getter);
            getter.Func.Method.Name.Should().Be(nameof(getter.GetValueReflection));
            getter.Func.Method.DeclaringType.Should().Be(typeof(StaticPropertyGetter<IBaz>));

            using (var gc = new GCNoRegion(4194304))
            {
                var reflectionTimer = Stopwatch.StartNew();
                IBaz reflectionValue = getter.GetValue();
                reflectionTimer.Stop();

                getter.SetOptimizedFunc();

                getter.Func.Target.Should().NotBeSameAs(getter);
                getter.Func.Method.Name.Should().Be(StaticPropertyGetter.GetStaticValueOptimized);
                getter.Func.Method.DeclaringType.Should().NotBe(typeof(StaticPropertyGetter<IBaz>));

                var optimizedTimer = Stopwatch.StartNew();
                IBaz optimizedValue = getter.GetValue();
                optimizedTimer.Stop();

                optimizedValue.Should().Be(reflectionValue);
                optimizedTimer.Elapsed.Should().BeLessThan(reflectionTimer.Elapsed);
            }
        }

        private class Foo
        {
            public Foo(string bar) => Bar = bar;
            public string Bar { get; }
        }

        private class Bar
        {
            public Baz Baz { get; set; }
        }

        private interface IBaz
        {
            int Qux { get; }
        }

        private struct Baz : IBaz
        {
            public Baz(int qux) => Qux = qux;
            public int Qux { get; }
        }

        private class Garply
        {
            private Garply() {}
            public static string Bar => "abc";
            public static Baz Baz { get; } = new Baz(456);
        }
    }
}
