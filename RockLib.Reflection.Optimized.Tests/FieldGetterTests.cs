using FluentAssertions;
using System;
using System.Diagnostics;
using System.Reflection;
using Xunit;

namespace RockLib.Reflection.Optimized.Tests
{
    public class FieldGetterTests
    {
        [Fact]
        public void FieldGetterWorks()
        {

            var foo = new Foo("abc");
            var field = typeof(Foo).GetField(nameof(Foo.Bar));

            var getter = new FieldGetter(field!);

            getter.Func.Target.Should().BeSameAs(field);
            getter.Func.Method.Name.Should().Be(nameof(field.GetValue));
            getter.Func.Method.DeclaringType.Should().BeAssignableTo(typeof(FieldInfo));

            using (var gc = new GCNoRegion(4194304))
            {
                var reflectionTimer = Stopwatch.StartNew();
                object reflectionValue = getter.GetValue(foo);
                reflectionTimer.Stop();

                getter.SetOptimizedFunc();

                getter.Func.Target.Should().NotBeSameAs(field);
                getter.Func.Method.Name.Should().Be(FieldGetter.GetValueOptimized);
                getter.Func.Method.DeclaringType.Should().NotBeAssignableTo(typeof(FieldInfo));

                var optimizedTimer = Stopwatch.StartNew();
                object optimizedValue = getter.GetValue(foo);
                optimizedTimer.Stop();

                optimizedValue.Should().Be(reflectionValue);
                optimizedTimer.Elapsed.Should().BeLessThan(reflectionTimer.Elapsed);
            }
        }

        [Fact]
        public void FieldGetter1Works()
        {
            var foo = new Foo("abc");
            var field = typeof(Foo).GetField(nameof(Foo.Bar));

            var getter = new FieldGetter<string>(field!);

            getter.Func.Target.Should().BeSameAs(getter);
            getter.Func.Method.Name.Should().Be(nameof(getter.GetValueReflection));
            getter.Func.Method.DeclaringType.Should().Be(typeof(FieldGetter<string>));

            using (var gc = new GCNoRegion(4194304))
            {
                var reflectionTimer = Stopwatch.StartNew();
                string reflectionValue = getter.GetValue(foo);
                reflectionTimer.Stop();

                getter.SetOptimizedFunc();

                getter.Func.Target.Should().NotBeSameAs(getter);
                getter.Func.Method.Name.Should().Be(FieldGetter.GetValueOptimized);
                getter.Func.Method.DeclaringType.Should().NotBe(typeof(FieldGetter<string>));

                var optimizedTimer = Stopwatch.StartNew();
                string optimizedValue = getter.GetValue(foo);
                optimizedTimer.Stop();

                optimizedValue.Should().Be(reflectionValue);
                optimizedTimer.Elapsed.Should().BeLessThan(reflectionTimer.Elapsed);
            }
        }

        [Fact]
        public void FieldGetter2Works()
        {
            var foo = new Foo("abc");
            var field = typeof(Foo).GetField(nameof(Foo.Bar));

            var getter = new FieldGetter<Foo, string>(field!);

            getter.Func.Target.Should().BeSameAs(getter);
            getter.Func.Method.Name.Should().Be(nameof(getter.GetValueReflection));
            getter.Func.Method.DeclaringType.Should().Be(typeof(FieldGetter<Foo, string>));

            using (var gc = new GCNoRegion(4194304))
            {
                var reflectionTimer = Stopwatch.StartNew();
                string reflectionValue = getter.GetValue(foo);
                reflectionTimer.Stop();

                getter.SetOptimizedFunc();

                getter.Func.Target.Should().NotBeSameAs(getter);
                getter.Func.Method.Name.Should().Be(FieldGetter.GetValueOptimized);
                getter.Func.Method.DeclaringType.Should().NotBe(typeof(FieldGetter<Foo, string>));

                var optimizedTimer = Stopwatch.StartNew();
                string optimizedValue = getter.GetValue(foo);
                optimizedTimer.Stop();

                optimizedValue.Should().Be(reflectionValue);
                optimizedTimer.Elapsed.Should().BeLessThan(reflectionTimer.Elapsed);
            }
        }

        [Fact]
        public void FieldGetterWorksWithBoxing()
        {
            var bar = new Bar { Baz = new Baz(123) };
            var field = typeof(Bar).GetField(nameof(Bar.Baz));

            var getter = new FieldGetter(field!);

            getter.Func.Target.Should().BeSameAs(field);
            getter.Func.Method.Name.Should().Be(nameof(field.GetValue));
            getter.Func.Method.DeclaringType.Should().BeAssignableTo(typeof(FieldInfo));

            using (var gc = new GCNoRegion(4194304))
            {
                var reflectionTimer = Stopwatch.StartNew();
                object reflectionValue = getter.GetValue(bar);
                reflectionTimer.Stop();

                getter.SetOptimizedFunc();

                getter.Func.Target.Should().NotBeSameAs(field);
                getter.Func.Method.Name.Should().Be(FieldGetter.GetValueOptimized);
                getter.Func.Method.DeclaringType.Should().NotBeAssignableTo(typeof(FieldInfo));

                var optimizedTimer = Stopwatch.StartNew();
                object optimizedValue = getter.GetValue(bar);
                optimizedTimer.Stop();

                optimizedValue.Should().Be(reflectionValue);
                optimizedTimer.Elapsed.Should().BeLessThan(reflectionTimer.Elapsed);
            }
        }

        [Fact]
        public void FieldGetter1WorksWithBoxing()
        {
            var bar = new Bar { Baz = new Baz(123) };
            var field = typeof(Bar).GetField(nameof(Bar.Baz));

            var getter = new FieldGetter<IBaz>(field!);

            getter.Func.Target.Should().BeSameAs(getter);
            getter.Func.Method.Name.Should().Be(nameof(getter.GetValueReflection));
            getter.Func.Method.DeclaringType.Should().Be(typeof(FieldGetter<IBaz>));

            using (var gc = new GCNoRegion(4194304))
            {
                var reflectionTimer = Stopwatch.StartNew();
                IBaz reflectionValue = getter.GetValue(bar);
                reflectionTimer.Stop();

                getter.SetOptimizedFunc();

                getter.Func.Target.Should().NotBeSameAs(getter);
                getter.Func.Method.Name.Should().Be(FieldGetter.GetValueOptimized);
                getter.Func.Method.DeclaringType.Should().NotBe(typeof(FieldGetter<IBaz>));

                var optimizedTimer = Stopwatch.StartNew();
                IBaz optimizedValue = getter.GetValue(bar);
                optimizedTimer.Stop();

                optimizedValue.Should().Be(reflectionValue);
                optimizedTimer.Elapsed.Should().BeLessThan(reflectionTimer.Elapsed);
            }
        }

        [Fact]
        public void FieldGetter2WorksWithBoxing()
        {
            var bar = new Bar { Baz = new Baz(123) };
            var field = typeof(Bar).GetField(nameof(Bar.Baz));

            var getter = new FieldGetter<Bar, IBaz>(field!);

            getter.Func.Target.Should().BeSameAs(getter);
            getter.Func.Method.Name.Should().Be(nameof(getter.GetValueReflection));
            getter.Func.Method.DeclaringType.Should().Be(typeof(FieldGetter<Bar, IBaz>));

            using (var gc = new GCNoRegion(4194304))
            {
                var reflectionTimer = Stopwatch.StartNew();
                IBaz reflectionValue = getter.GetValue(bar);
                reflectionTimer.Stop();

                getter.SetOptimizedFunc();

                getter.Func.Target.Should().NotBeSameAs(getter);
                getter.Func.Method.Name.Should().Be(FieldGetter.GetValueOptimized);
                getter.Func.Method.DeclaringType.Should().NotBe(typeof(FieldGetter<Bar, IBaz>));

                var optimizedTimer = Stopwatch.StartNew();
                IBaz optimizedValue = getter.GetValue(bar);
                optimizedTimer.Stop();

                optimizedValue.Should().Be(reflectionValue);
                optimizedTimer.Elapsed.Should().BeLessThan(reflectionTimer.Elapsed);
            }
        }

        [Fact]
        public void FieldGetterWorksWithStatic()
        {
            var field = typeof(Garply).GetField(nameof(Garply.Bar));

            var getter = new FieldGetter(field!);

            getter.Func.Target.Should().BeSameAs(field);
            getter.Func.Method.Name.Should().Be(nameof(field.GetValue));
            getter.Func.Method.DeclaringType.Should().BeAssignableTo(typeof(FieldInfo));

            using (var gc = new GCNoRegion(4194304))
            {
                var reflectionTimer = Stopwatch.StartNew();
                object reflectionValue = getter.GetValue(null!);
                reflectionTimer.Stop();

                getter.SetOptimizedFunc();

                getter.Func.Target.Should().NotBeSameAs(field);
                getter.Func.Method.Name.Should().Be(FieldGetter.GetValueOptimized);
                getter.Func.Method.DeclaringType.Should().NotBeAssignableTo(typeof(FieldInfo));

                var optimizedTimer = Stopwatch.StartNew();
                object optimizedValue = getter.GetValue(null!);
                optimizedTimer.Stop();

                optimizedValue.Should().Be(reflectionValue);
                optimizedTimer.Elapsed.Should().BeLessThan(reflectionTimer.Elapsed);
            }
        }

        [Fact]
        public void FieldGetter1WorksWithStatic()
        {
            var field = typeof(Garply).GetField(nameof(Garply.Bar));

            var getter = new FieldGetter<string>(field!);

            getter.Func.Target.Should().BeSameAs(getter);
            getter.Func.Method.Name.Should().Be(nameof(getter.GetValueReflection));
            getter.Func.Method.DeclaringType.Should().Be(typeof(FieldGetter<string>));

            using (var gc = new GCNoRegion(4194304))
            {
                var reflectionTimer = Stopwatch.StartNew();
                string reflectionValue = getter.GetValue(null!);
                reflectionTimer.Stop();

                getter.SetOptimizedFunc();

                getter.Func.Target.Should().NotBeSameAs(getter);
                getter.Func.Method.Name.Should().Be(FieldGetter.GetValueOptimized);
                getter.Func.Method.DeclaringType.Should().NotBe(typeof(FieldGetter<string>));

                var optimizedTimer = Stopwatch.StartNew();
                string optimizedValue = getter.GetValue(null!);
                optimizedTimer.Stop();

                optimizedValue.Should().Be(reflectionValue);
                optimizedTimer.Elapsed.Should().BeLessThan(reflectionTimer.Elapsed);
            }
        }

        [Fact]
        public void FieldGetter2WorksWithStatic()
        {
            var field = typeof(Garply).GetField(nameof(Garply.Bar));

            var getter = new FieldGetter<Garply, string>(field!);

            getter.Func.Target.Should().BeSameAs(getter);
            getter.Func.Method.Name.Should().Be(nameof(getter.GetValueReflection));
            getter.Func.Method.DeclaringType.Should().Be(typeof(FieldGetter<Garply, string>));

            using (var gc = new GCNoRegion(4194304))
            {
                var reflectionTimer = Stopwatch.StartNew();
                string reflectionValue = getter.GetValue(null!);
                reflectionTimer.Stop();

                getter.SetOptimizedFunc();

                getter.Func.Target.Should().NotBeSameAs(getter);
                getter.Func.Method.Name.Should().Be(FieldGetter.GetValueOptimized);
                getter.Func.Method.DeclaringType.Should().NotBe(typeof(FieldGetter<Garply, string>));

                var optimizedTimer = Stopwatch.StartNew();
                string optimizedValue = getter.GetValue(null!);
                optimizedTimer.Stop();

                optimizedValue.Should().Be(reflectionValue);
                optimizedTimer.Elapsed.Should().BeLessThan(reflectionTimer.Elapsed);
            }
        }

        [Fact]
        public void StaticFieldGetterWorks()
        {
            var field = typeof(Garply).GetField(nameof(Garply.Bar));

            var getter = new StaticFieldGetter(field!);

            getter.Func.Target.Should().BeSameAs(getter);
            getter.Func.Method.Name.Should().Be(nameof(getter.GetValueReflection));
            getter.Func.Method.DeclaringType.Should().Be(typeof(StaticFieldGetter));

            using (var gc = new GCNoRegion(4194304))
            {
                var reflectionTimer = Stopwatch.StartNew();
                object reflectionValue = getter.GetValue();
                reflectionTimer.Stop();

                getter.SetOptimizedFunc();

                getter.Func.Target.Should().NotBeSameAs(getter);
                getter.Func.Method.Name.Should().Be(StaticFieldGetter.GetStaticValueOptimized);
                getter.Func.Method.DeclaringType.Should().NotBe(typeof(StaticFieldGetter));

                var optimizedTimer = Stopwatch.StartNew();
                object optimizedValue = getter.GetValue();
                optimizedTimer.Stop();

                optimizedValue.Should().Be(reflectionValue);
                optimizedTimer.Elapsed.Should().BeLessThan(reflectionTimer.Elapsed);
            }
        }

        [Fact]
        public void StaticFieldGetter1Works()
        {
            var field = typeof(Garply).GetField(nameof(Garply.Bar));

            var getter = new StaticFieldGetter<string>(field!);

            getter.Func.Target.Should().BeSameAs(getter);
            getter.Func.Method.Name.Should().Be(nameof(getter.GetValueReflection));
            getter.Func.Method.DeclaringType.Should().Be(typeof(StaticFieldGetter<string>));

            using (var gc = new GCNoRegion(4194304))
            {
                var reflectionTimer = Stopwatch.StartNew();
                string reflectionValue = getter.GetValue();
                reflectionTimer.Stop();

                getter.SetOptimizedFunc();

                getter.Func.Target.Should().NotBeSameAs(getter);
                getter.Func.Method.Name.Should().Be(StaticFieldGetter.GetStaticValueOptimized);
                getter.Func.Method.DeclaringType.Should().NotBe(typeof(StaticFieldGetter<string>));

                var optimizedTimer = Stopwatch.StartNew();
                string optimizedValue = getter.GetValue();
                optimizedTimer.Stop();

                optimizedValue.Should().Be(reflectionValue);
                optimizedTimer.Elapsed.Should().BeLessThan(reflectionTimer.Elapsed);
            }
        }

        [Fact]
        public void StaticFieldGetterWorksWithBoxing()
        {
            var field = typeof(Garply).GetField(nameof(Garply.Baz));

            var getter = new StaticFieldGetter(field!);

            getter.Func.Target.Should().BeSameAs(getter);
            getter.Func.Method.Name.Should().Be(nameof(getter.GetValueReflection));
            getter.Func.Method.DeclaringType.Should().Be(typeof(StaticFieldGetter));

            using (var gc = new GCNoRegion(4194304))
            {
                var reflectionTimer = Stopwatch.StartNew();
                object reflectionValue = getter.GetValue();
                reflectionTimer.Stop();

                getter.SetOptimizedFunc();

                getter.Func.Target.Should().NotBeSameAs(getter);
                getter.Func.Method.Name.Should().Be(StaticFieldGetter.GetStaticValueOptimized);
                getter.Func.Method.DeclaringType.Should().NotBe(typeof(StaticFieldGetter));

                var optimizedTimer = Stopwatch.StartNew();
                object optimizedValue = getter.GetValue();
                optimizedTimer.Stop();

                optimizedValue.Should().Be(reflectionValue);
                optimizedTimer.Elapsed.Should().BeLessThan(reflectionTimer.Elapsed);
            }
        }

        [Fact]
        public void StaticFieldGetter1WorksWithBoxing()
        {
            var field = typeof(Garply).GetField(nameof(Garply.Baz));

            var getter = new StaticFieldGetter<IBaz>(field!);

            getter.Func.Target.Should().BeSameAs(getter);
            getter.Func.Method.Name.Should().Be(nameof(getter.GetValueReflection));
            getter.Func.Method.DeclaringType.Should().Be(typeof(StaticFieldGetter<IBaz>));

            using (var gc = new GCNoRegion(4194304))
            {
                var reflectionTimer = Stopwatch.StartNew();
                IBaz reflectionValue = getter.GetValue();
                reflectionTimer.Stop();

                getter.SetOptimizedFunc();

                getter.Func.Target.Should().NotBeSameAs(getter);
                getter.Func.Method.Name.Should().Be(StaticFieldGetter.GetStaticValueOptimized);
                getter.Func.Method.DeclaringType.Should().NotBe(typeof(StaticFieldGetter<IBaz>));

                var optimizedTimer = Stopwatch.StartNew();
                IBaz optimizedValue = getter.GetValue();
                optimizedTimer.Stop();

                optimizedValue.Should().Be(reflectionValue);
                optimizedTimer.Elapsed.Should().BeLessThan(reflectionTimer.Elapsed);
            }
        }

        private sealed class Foo
        {
            public Foo(string bar) => Bar = bar;
            public string Bar;
        }

        private sealed class Bar
        {
            public Baz Baz;
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

#pragma warning disable CA1812
        private sealed class Garply
#pragma warning restore CA1812
        {
            private Garply() {}
            public static string Bar = "abc";
            public static Baz Baz = new Baz(456);
        }
    }
}
