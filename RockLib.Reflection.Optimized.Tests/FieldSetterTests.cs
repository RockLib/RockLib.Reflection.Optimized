using FluentAssertions;
using System;
using System.Diagnostics;
using System.Reflection;
using Xunit;

namespace RockLib.Reflection.Optimized.Tests
{
    public class FieldSetterTests
    {
        private static readonly FieldInfo _field = typeof(Foo).GetField(nameof(Foo.Bar));

        [Fact]
        public void FieldSetterWorks()
        {
            var foo = new Foo();

            var setter = new FieldSetter(_field);

            setter.Action.Target.Should().BeSameAs(_field);
            setter.Action.Method.Name.Should().Be(nameof(_field.SetValue));
            setter.Action.Method.DeclaringType.Should().Be(typeof(FieldInfo));

            if (GC.TryStartNoGCRegion(4194304, true))
            {
                try
                {
                    var reflectionTimer = Stopwatch.StartNew();
                    setter.SetValue(foo, 123);
                    reflectionTimer.Stop();

                    foo.Bar.Should().Be(123);

                    setter.SetOptimizedAction();

                    setter.Action.Target.Should().NotBeSameAs(_field);
                    setter.Action.Method.Name.Should().Be(FieldSetter.SetValueOptimized);
                    setter.Action.Method.DeclaringType.Should().NotBe(typeof(FieldInfo));

                    var optimizedTimer = Stopwatch.StartNew();
                    setter.SetValue(foo, 456);
                    optimizedTimer.Stop();

                    foo.Bar.Should().Be(456);

                    optimizedTimer.Elapsed.Should().BeLessThan(reflectionTimer.Elapsed);
                }
                finally
                {
                    GC.EndNoGCRegion();
                }
            }
            else
                throw new Exception("GC was not able to commit the required amount of memory and the garbage collector was not able to enter no GC region latency mode.");
        }

        [Fact]
        public void FieldSetter1Works()
        {
            var foo = new Foo();

            var setter = new FieldSetter<int>(_field);

            setter.Action.Target.Should().BeSameAs(setter);
            setter.Action.Method.Name.Should().Be(nameof(setter.SetValueReflection));
            setter.Action.Method.DeclaringType.Should().Be(typeof(FieldSetter<int>));

            if (GC.TryStartNoGCRegion(4194304, true))
            {
                try
                {
                    var reflectionTimer = Stopwatch.StartNew();
                    setter.SetValue(foo, 123);
                    reflectionTimer.Stop();

                    foo.Bar.Should().Be(123);

                    setter.SetOptimizedAction();

                    setter.Action.Target.Should().NotBeSameAs(setter);
                    setter.Action.Method.Name.Should().Be(FieldSetter.SetValueOptimized);
                    setter.Action.Method.DeclaringType.Should().NotBe(typeof(FieldSetter<int>));

                    var optimizedTimer = Stopwatch.StartNew();
                    setter.SetValue(foo, 456);
                    optimizedTimer.Stop();

                    foo.Bar.Should().Be(456);

                    optimizedTimer.Elapsed.Should().BeLessThan(reflectionTimer.Elapsed);
                }
                finally
                {
                    GC.EndNoGCRegion();
                }
            }
            else
                throw new Exception("GC was not able to commit the required amount of memory and the garbage collector was not able to enter no GC region latency mode.");
        }

        [Fact]
        public void FieldSetter2Works()
        {
            var foo = new Foo();

            var setter = new FieldSetter<Foo, int>(_field);

            setter.Action.Target.Should().BeSameAs(setter);
            setter.Action.Method.Name.Should().Be(nameof(setter.SetValueReflection));
            setter.Action.Method.DeclaringType.Should().Be(typeof(FieldSetter<Foo, int>));

            if (GC.TryStartNoGCRegion(4194304, true))
            {
                try
                {
                    var reflectionTimer = Stopwatch.StartNew();
                    setter.SetValue(foo, 123);
                    reflectionTimer.Stop();

                    foo.Bar.Should().Be(123);

                    setter.SetOptimizedAction();

                    setter.Action.Target.Should().NotBeSameAs(setter);
                    setter.Action.Method.Name.Should().Be(FieldSetter.SetValueOptimized);
                    setter.Action.Method.DeclaringType.Should().NotBe(typeof(FieldSetter<Foo, int>));

                    var optimizedTimer = Stopwatch.StartNew();
                    setter.SetValue(foo, 456);
                    optimizedTimer.Stop();

                    foo.Bar.Should().Be(456);

                    optimizedTimer.Elapsed.Should().BeLessThan(reflectionTimer.Elapsed);
                }
                finally
                {
                    GC.EndNoGCRegion();
                }
            }
            else
                throw new Exception("GC was not able to commit the required amount of memory and the garbage collector was not able to enter no GC region latency mode.");
        }

        [Fact]
        public void FieldSetterWorksWithStatic()
        {
            Baz.Qux = -1;

            var field = typeof(Baz).GetField(nameof(Baz.Qux));

            var setter = new FieldSetter(field);

            setter.Action.Target.Should().BeSameAs(field);
            setter.Action.Method.Name.Should().Be(nameof(field.SetValue));
            setter.Action.Method.DeclaringType.Should().Be(typeof(FieldInfo));

            if (GC.TryStartNoGCRegion(4194304, true))
            {
                try
                {
                    var reflectionTimer = Stopwatch.StartNew();
                    setter.SetValue(null, 123);
                    reflectionTimer.Stop();

                    Baz.Qux.Should().Be(123);

                    setter.SetOptimizedAction();

                    setter.Action.Target.Should().NotBeSameAs(field);
                    setter.Action.Method.Name.Should().Be(FieldSetter.SetValueOptimized);
                    setter.Action.Method.DeclaringType.Should().NotBe(typeof(FieldInfo));

                    var optimizedTimer = Stopwatch.StartNew();
                    setter.SetValue(null, 456);
                    optimizedTimer.Stop();

                    Baz.Qux.Should().Be(456);

                    optimizedTimer.Elapsed.Should().BeLessThan(reflectionTimer.Elapsed);


                }
                finally
                {
                    GC.EndNoGCRegion();
                }
            }
            else
                throw new Exception("GC was not able to commit the required amount of memory and the garbage collector was not able to enter no GC region latency mode.");
        }

        [Fact]
        public void FieldSetter1WorksWithStatic()
        {
            Baz.Qux = -1;

            var field = typeof(Baz).GetField(nameof(Baz.Qux));

            var setter = new FieldSetter<int>(field);

            setter.Action.Target.Should().BeSameAs(setter);
            setter.Action.Method.Name.Should().Be(nameof(setter.SetValueReflection));
            setter.Action.Method.DeclaringType.Should().Be(typeof(FieldSetter<int>));

            if (GC.TryStartNoGCRegion(4194304, true))
            {
                try
                {
                    var reflectionTimer = Stopwatch.StartNew();
                    setter.SetValue(null, 123);
                    reflectionTimer.Stop();

                    Baz.Qux.Should().Be(123);

                    setter.SetOptimizedAction();

                    setter.Action.Target.Should().NotBeSameAs(setter);
                    setter.Action.Method.Name.Should().Be(FieldSetter.SetValueOptimized);
                    setter.Action.Method.DeclaringType.Should().NotBe(typeof(FieldSetter<int>));

                    var optimizedTimer = Stopwatch.StartNew();
                    setter.SetValue(null, 456);
                    optimizedTimer.Stop();

                    Baz.Qux.Should().Be(456);

                    optimizedTimer.Elapsed.Should().BeLessThan(reflectionTimer.Elapsed);
                }
                finally
                {
                    GC.EndNoGCRegion();
                }
            }
            else
                throw new Exception("GC was not able to commit the required amount of memory and the garbage collector was not able to enter no GC region latency mode.");
        }

        [Fact]
        public void FieldSetter2WorksWithStatic()
        {
            Baz.Qux = -1;

            var field = typeof(Baz).GetField(nameof(Baz.Qux));

            var setter = new FieldSetter<Foo, int>(field);

            setter.Action.Target.Should().BeSameAs(setter);
            setter.Action.Method.Name.Should().Be(nameof(setter.SetValueReflection));
            setter.Action.Method.DeclaringType.Should().Be(typeof(FieldSetter<Foo, int>));

            if (GC.TryStartNoGCRegion(4194304, true))
            {
                try
                {
                    var reflectionTimer = Stopwatch.StartNew();
                    setter.SetValue(null, 123);
                    reflectionTimer.Stop();

                    Baz.Qux.Should().Be(123);

                    setter.SetOptimizedAction();

                    setter.Action.Target.Should().NotBeSameAs(setter);
                    setter.Action.Method.Name.Should().Be(FieldSetter.SetValueOptimized);
                    setter.Action.Method.DeclaringType.Should().NotBe(typeof(FieldSetter<Foo, int>));

                    var optimizedTimer = Stopwatch.StartNew();
                    setter.SetValue(null, 456);
                    optimizedTimer.Stop();

                    Baz.Qux.Should().Be(456);

                    optimizedTimer.Elapsed.Should().BeLessThan(reflectionTimer.Elapsed);
                }
                finally
                {
                    GC.EndNoGCRegion();
                }
            }
            else
                throw new Exception("GC was not able to commit the required amount of memory and the garbage collector was not able to enter no GC region latency mode.");
        }

        [Fact]
        public void StaticFieldSetterWorks()
        {
            Baz.Qux = -1;

            var field = typeof(Baz).GetField(nameof(Baz.Qux));

            var setter = new StaticFieldSetter(field);

            setter.Action.Target.Should().BeSameAs(setter);
            setter.Action.Method.Name.Should().Be(nameof(setter.SetValueReflection));
            setter.Action.Method.DeclaringType.Should().Be(typeof(StaticFieldSetter));

            if (GC.TryStartNoGCRegion(4194304, true))
            {
                try
                {
                    var reflectionTimer = Stopwatch.StartNew();
                    setter.SetValue(123);
                    reflectionTimer.Stop();

                    Baz.Qux.Should().Be(123);

                    setter.SetOptimizedAction();

                    setter.Action.Target.Should().NotBeSameAs(setter);
                    setter.Action.Method.Name.Should().Be(StaticFieldSetter.SetStaticValueOptimized);
                    setter.Action.Method.DeclaringType.Should().NotBe(typeof(StaticFieldSetter));

                    var optimizedTimer = Stopwatch.StartNew();
                    setter.SetValue(456);
                    optimizedTimer.Stop();

                    Baz.Qux.Should().Be(456);

                    optimizedTimer.Elapsed.Should().BeLessThan(reflectionTimer.Elapsed);
                }
                finally
                {
                    GC.EndNoGCRegion();
                }
            }
            else
                throw new Exception("GC was not able to commit the required amount of memory and the garbage collector was not able to enter no GC region latency mode.");
        }

        [Fact]
        public void StaticFieldSetter1Works()
        {
            Baz.Qux = -1;

            var field = typeof(Baz).GetField(nameof(Baz.Qux));

            var setter = new StaticFieldSetter<int>(field);

            setter.Action.Target.Should().BeSameAs(setter);
            setter.Action.Method.Name.Should().Be(nameof(setter.SetValueReflection));
            setter.Action.Method.DeclaringType.Should().Be(typeof(StaticFieldSetter<int>));

            if (GC.TryStartNoGCRegion(4194304, true))
            {
                try
                {
                    var reflectionTimer = Stopwatch.StartNew();
                    setter.SetValue(123);
                    reflectionTimer.Stop();

                    Baz.Qux.Should().Be(123);

                    setter.SetOptimizedAction();

                    setter.Action.Target.Should().NotBeSameAs(setter);
                    setter.Action.Method.Name.Should().Be(StaticFieldSetter.SetStaticValueOptimized);
                    setter.Action.Method.DeclaringType.Should().NotBe(typeof(StaticFieldSetter<int>));

                    var optimizedTimer = Stopwatch.StartNew();
                    setter.SetValue(456);
                    optimizedTimer.Stop();

                    Baz.Qux.Should().Be(456);

                    optimizedTimer.Elapsed.Should().BeLessThan(reflectionTimer.Elapsed);
                }
                finally
                {
                    GC.EndNoGCRegion();
                }
            }
            else
                throw new Exception("GC was not able to commit the required amount of memory and the garbage collector was not able to enter no GC region latency mode.");
        }

        public class Foo
        {
            public int Bar;
        }

        public class Baz
        {
            public static int Qux = -1;
        }
    }
}
