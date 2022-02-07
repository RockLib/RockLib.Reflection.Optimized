using FluentAssertions;
using System;
using System.Reflection;
using System.Threading;
using Xunit;

namespace RockLib.Reflection.Optimized.Tests
{
    public class FieldInfoExtensionsTests
    {
        private static readonly FieldInfo _field = typeof(Foo).GetField(nameof(Foo.Bar));

        [Fact]
        public void CreateGetterThrowsIfFieldParameterIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => FieldInfoExtensions.CreateGetter(null!));
            exception.ParamName.Should().Be("field");
        }

        [Fact]
        public void CreateGetter1ThrowsIfFieldParameterIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => FieldInfoExtensions.CreateGetter<int>(null!));
            exception.ParamName.Should().Be("field");
        }

        [Fact]
        public void CreateGetter1ThrowsIfTheTypeArgumentIsNotCompatibleWithTheFieldType()
        {
            var field = typeof(Foo).GetField(nameof(Foo.Bar));
            var exception = Assert.Throws<ArgumentException>(() => field.CreateGetter<string>());
            exception.ParamName.Should().Be("field");
        }

        [Fact]
        public void CreateGetter2ThrowsIfFieldParameterIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => FieldInfoExtensions.CreateGetter<Foo, int>(null!));
            exception.ParamName.Should().Be("field");
        }

        [Fact]
        public void CreateGetter2ThrowsIfTheFirstTypeArgumentIsNotCompatibleWithTheFieldDeclaringType()
        {
            var field = typeof(Foo).GetField(nameof(Foo.Bar));
            var exception = Assert.Throws<ArgumentException>(() => field.CreateGetter<Fred, int>());
            exception.ParamName.Should().Be("field");
        }

        [Fact]
        public void CreateGetter2ThrowsIfTheSecondTypeArgumentIsNotCompatibleWithTheFieldType()
        {
            var field = typeof(Foo).GetField(nameof(Foo.Bar));
            var exception = Assert.Throws<ArgumentException>(() => field.CreateGetter<Foo, string>());
            exception.ParamName.Should().Be("field");
        }

        [Fact]
        public void CreateSetterThrowsIfFieldParameterIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => FieldInfoExtensions.CreateSetter(null!));
            exception.ParamName.Should().Be("field");
        }

        [Fact]
        public void CreateSetterThrowsIfFieldParameterIsReadonly()
        {
            var field = typeof(Foo).GetField(nameof(Foo.Grault));
            var exception = Assert.Throws<ArgumentException>(() => field.CreateSetter());
            exception.ParamName.Should().Be("field");
        }

        [Fact]
        public void CreateSetter1ThrowsIfFieldParameterIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => FieldInfoExtensions.CreateSetter<int>(null!));
            exception.ParamName.Should().Be("field");
        }

        [Fact]
        public void CreateSetter1ThrowsIfTheTypeArgumentIsNotCompatibleWithTheFieldType()
        {
            var field = typeof(Foo).GetField(nameof(Foo.Bar));
            var exception = Assert.Throws<ArgumentException>(() => field.CreateSetter<string>());
            exception.ParamName.Should().Be("field");
        }

        [Fact]
        public void CreateSetter1ThrowsIfFieldParameterIsReadonly()
        {
            var field = typeof(Foo).GetField(nameof(Foo.Grault));
            var exception = Assert.Throws<ArgumentException>(() => field.CreateSetter<int>());
            exception.ParamName.Should().Be("field");
        }

        [Fact]
        public void CreateSetter2ThrowsIfFieldParameterIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => FieldInfoExtensions.CreateSetter<Foo, int>(null!));
            exception.ParamName.Should().Be("field");
        }

        [Fact]
        public void CreateSetter2ThrowsIfTheFirstTypeArgumentIsNotCompatibleWithTheFieldDeclaringType()
        {
            var field = typeof(Foo).GetField(nameof(Foo.Bar));
            var exception = Assert.Throws<ArgumentException>(() => field.CreateSetter<Fred, int>());
            exception.ParamName.Should().Be("field");
        }

        [Fact]
        public void CreateSetter2ThrowsIfTheSecondTypeArgumentIsNotCompatibleWithTheFieldType()
        {
            var field = typeof(Foo).GetField(nameof(Foo.Bar));
            var exception = Assert.Throws<ArgumentException>(() => field.CreateSetter<Foo, string>());
            exception.ParamName.Should().Be("field");
        }

        [Fact]
        public void CreateSetter2ThrowsIfFieldParameterIsReadonly()
        {
            var field = typeof(Foo).GetField(nameof(Foo.Grault));
            var exception = Assert.Throws<ArgumentException>(() => field.CreateSetter<Foo, int>());
            exception.ParamName.Should().Be("field");
        }

        [Fact]
        public void CreateStaticGetterThrowsIfFieldParameterIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => FieldInfoExtensions.CreateStaticGetter(null!));
            exception.ParamName.Should().Be("field");
        }

        [Fact]
        public void CreateStaticGetterThrowsIfFieldParameterIsNotStatic()
        {
            var field = typeof(Foo).GetField(nameof(Foo.Bar));
            var exception = Assert.Throws<ArgumentException>(() => field.CreateStaticGetter());
            exception.ParamName.Should().Be("field");
        }

        [Fact]
        public void CreateStaticGetter1ThrowsIfFieldParameterIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => FieldInfoExtensions.CreateStaticGetter<int>(null!));
            exception.ParamName.Should().Be("field");
        }

        [Fact]
        public void CreateStaticGetter1ThrowsIfTheTypeArgumentIsNotCompatibleWithTheFieldType()
        {
            var field = typeof(Corge).GetField(nameof(Corge.Bar));
            var exception = Assert.Throws<ArgumentException>(() => field.CreateStaticGetter<string>());
            exception.ParamName.Should().Be("field");
        }

        [Fact]
        public void CreateStaticGetter1ThrowsIfFieldParameterIsNotStatic()
        {
            var field = typeof(Foo).GetField(nameof(Foo.Bar));
            var exception = Assert.Throws<ArgumentException>(() => field.CreateStaticGetter<int>());
            exception.ParamName.Should().Be("field");
        }

        [Fact]
        public void CreateStaticSetterThrowsIfFieldParameterIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => FieldInfoExtensions.CreateStaticSetter(null!));
            exception.ParamName.Should().Be("field");
        }

        [Fact]
        public void CreateStaticSetterThrowsIfFieldParameterIsReadonly()
        {
            var field = typeof(Corge).GetField(nameof(Corge.Grault));
            var exception = Assert.Throws<ArgumentException>(() => field.CreateStaticSetter());
            exception.ParamName.Should().Be("field");
        }

        [Fact]
        public void CreateStaticSetterThrowsIfFieldParameterIsNotStatic()
        {
            var field = typeof(Foo).GetField(nameof(Foo.Bar));
            var exception = Assert.Throws<ArgumentException>(() => field.CreateStaticSetter());
            exception.ParamName.Should().Be("field");
        }

        [Fact]
        public void CreateStaticSetter1ThrowsIfFieldParameterIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => FieldInfoExtensions.CreateStaticSetter<int>(null!));
            exception.ParamName.Should().Be("field");
        }

        [Fact]
        public void CreateStaticSetter1ThrowsIfTheTypeArgumentIsNotCompatibleWithTheFieldType()
        {
            var field = typeof(Corge).GetField(nameof(Corge.Bar));
            var exception = Assert.Throws<ArgumentException>(() => field.CreateStaticSetter<string>());
            exception.ParamName.Should().Be("field");
        }

        [Fact]
        public void CreateStaticSetter1ThrowsIfFieldParameterIsReadonly()
        {
            var field = typeof(Corge).GetField(nameof(Corge.Grault));
            var exception = Assert.Throws<ArgumentException>(() => field.CreateStaticSetter<int>());
            exception.ParamName.Should().Be("field");
        }

        [Fact]
        public void CreateStaticSetter1ThrowsIfFieldParameterIsNotStatic()
        {
            var field = typeof(Foo).GetField(nameof(Foo.Bar));
            var exception = Assert.Throws<ArgumentException>(() => field.CreateStaticSetter<int>());
            exception.ParamName.Should().Be("field");
        }

        [Fact]
        public void CreateGetterWorks()
        {
            Func<object, object> getter;
            var queued = (Callback: (WaitCallback)null!, FieldGetter: (FieldGetter)null!);

            void queueUserWorkItem(WaitCallback callback, object state) => queued = (callback, (FieldGetter)state);

            FieldInfoExtensions.SetQueueUserWorkItemAction(queueUserWorkItem);

            try
            {
                getter = _field.CreateGetter();
            }
            finally
            {
                FieldInfoExtensions.SetQueueUserWorkItemAction(null!);
            }

            // Verify that a work item was queued
            queued.FieldGetter.Should().NotBeNull();
            queued.Callback.Should().NotBeNull();

            // Verify that the return delegate is correct
            getter.Target.Should().BeSameAs(queued.FieldGetter);
            getter.Method.Name.Should().Be(nameof(FieldGetter.GetValue));
            getter.Method.DeclaringType.Should().Be(typeof(FieldGetter));

            // Verify that the queued Callback calls the SetOptimizedFunc
            // method of the queued FieldGetter.
            var beforeFunc = queued.FieldGetter.Func;
            queued.Callback.Invoke(queued.FieldGetter);
            var afterFunc = queued.FieldGetter.Func;

            beforeFunc.Should().NotBeSameAs(afterFunc);
        }

        [Fact]
        public void CreateGetter1Works()
        {
            Func<object, int> getter;
            var queued = (Callback: (WaitCallback)null!, FieldGetter: (FieldGetter<int>)null!);

            void queueUserWorkItem(WaitCallback callback, object state) => queued = (callback, (FieldGetter<int>)state);

            FieldInfoExtensions.SetQueueUserWorkItemAction(queueUserWorkItem);

            try
            {
                getter = _field.CreateGetter<int>();
            }
            finally
            {
                FieldInfoExtensions.SetQueueUserWorkItemAction(null!);
            }

            // Verify that a work item was queued
            queued.FieldGetter.Should().NotBeNull();
            queued.Callback.Should().NotBeNull();

            // Verify that the return delegate is correct
            getter.Target.Should().BeSameAs(queued.FieldGetter);
            getter.Method.Name.Should().Be(nameof(FieldGetter<int>.GetValue));
            getter.Method.DeclaringType.Should().Be(typeof(FieldGetter<int>));

            // Verify that the queued Callback calls the SetOptimizedFunc
            // method of the queued FieldGetter.
            var beforeFunc = queued.FieldGetter.Func;
            queued.Callback.Invoke(queued.FieldGetter);
            var afterFunc = queued.FieldGetter.Func;

            beforeFunc.Should().NotBeSameAs(afterFunc);
        }

        [Fact]
        public void CreateGetter2Works()
        {
            Func<Foo, int> getter;
            var queued = (Callback: (WaitCallback)null!, FieldGetter: (FieldGetter<Foo, int>)null!);

            void queueUserWorkItem(WaitCallback callback, object state) => queued = (callback, (FieldGetter<Foo, int>)state);

            FieldInfoExtensions.SetQueueUserWorkItemAction(queueUserWorkItem);

            try
            {
                getter = _field.CreateGetter<Foo, int>();
            }
            finally
            {
                FieldInfoExtensions.SetQueueUserWorkItemAction(null!);
            }

            // Verify that a work item was queued
            queued.FieldGetter.Should().NotBeNull();
            queued.Callback.Should().NotBeNull();

            // Verify that the return delegate is correct
            getter.Target.Should().BeSameAs(queued.FieldGetter);
            getter.Method.Name.Should().Be(nameof(FieldGetter<Foo, int>.GetValue));
            getter.Method.DeclaringType.Should().Be(typeof(FieldGetter<Foo, int>));

            // Verify that the queued Callback calls the SetOptimizedFunc
            // method of the queued FieldGetter.
            var beforeFunc = queued.FieldGetter.Func;
            queued.Callback.Invoke(queued.FieldGetter);
            var afterFunc = queued.FieldGetter.Func;

            beforeFunc.Should().NotBeSameAs(afterFunc);
        }

        [Fact]
        public void CreateSetterWorks()
        {
            Action<object, object> setter;
            var queued = (Callback: (WaitCallback)null!, FieldSetter: (FieldSetter)null!);

            void queueUserWorkItem(WaitCallback callback, object state) => queued = (callback, (FieldSetter)state);

            FieldInfoExtensions.SetQueueUserWorkItemAction(queueUserWorkItem);

            try
            {
                setter = _field.CreateSetter();
            }
            finally
            {
                FieldInfoExtensions.SetQueueUserWorkItemAction(null!);
            }

            // Verify that a work item was queued
            queued.FieldSetter.Should().NotBeNull();
            queued.Callback.Should().NotBeNull();

            // Verify that the return delegate is correct
            setter.Target.Should().BeSameAs(queued.FieldSetter);
            setter.Method.Name.Should().Be(nameof(FieldSetter.SetValue));
            setter.Method.DeclaringType.Should().Be(typeof(FieldSetter));

            // Verify that the queued Callback calls the SetOptimizedFunc
            // method of the queued FieldSetter.
            var beforeAction = queued.FieldSetter.Action;
            queued.Callback.Invoke(queued.FieldSetter);
            var afterAction = queued.FieldSetter.Action;

            beforeAction.Should().NotBeSameAs(afterAction);
        }

        [Fact]
        public void CreateSetter1Works()
        {
            Action<object, int> setter;
            var queued = (Callback: (WaitCallback)null!, FieldSetter: (FieldSetter<int>)null!);

            void queueUserWorkItem(WaitCallback callback, object state) => queued = (callback, (FieldSetter<int>)state);

            FieldInfoExtensions.SetQueueUserWorkItemAction(queueUserWorkItem);

            try
            {
                setter = _field.CreateSetter<int>();
            }
            finally
            {
                FieldInfoExtensions.SetQueueUserWorkItemAction(null!);
            }

            // Verify that a work item was queued
            queued.FieldSetter.Should().NotBeNull();
            queued.Callback.Should().NotBeNull();

            // Verify that the return delegate is correct
            setter.Target.Should().BeSameAs(queued.FieldSetter);
            setter.Method.Name.Should().Be(nameof(FieldSetter<int>.SetValue));
            setter.Method.DeclaringType.Should().Be(typeof(FieldSetter<int>));

            // Verify that the queued Callback calls the SetOptimizedFunc
            // method of the queued FieldSetter.
            var beforeAction = queued.FieldSetter.Action;
            queued.Callback.Invoke(queued.FieldSetter);
            var afterAction = queued.FieldSetter.Action;

            beforeAction.Should().NotBeSameAs(afterAction);
        }

        [Fact]
        public void CreateSetter2Works()
        {
            Action<Foo, int> setter;
            var queued = (Callback: (WaitCallback)null!, FieldSetter: (FieldSetter<Foo, int>)null!);

            void queueUserWorkItem(WaitCallback callback, object state) => queued = (callback, (FieldSetter<Foo, int>)state);

            FieldInfoExtensions.SetQueueUserWorkItemAction(queueUserWorkItem);

            try
            {
                setter = _field.CreateSetter<Foo, int>();
            }
            finally
            {
                FieldInfoExtensions.SetQueueUserWorkItemAction(null!);
            }

            // Verify that a work item was queued
            queued.FieldSetter.Should().NotBeNull();
            queued.Callback.Should().NotBeNull();

            // Verify that the return delegate is correct
            setter.Target.Should().BeSameAs(queued.FieldSetter);
            setter.Method.Name.Should().Be(nameof(FieldSetter<Foo, int>.SetValue));
            setter.Method.DeclaringType.Should().Be(typeof(FieldSetter<Foo, int>));

            // Verify that the queued Callback calls the SetOptimizedFunc
            // method of the queued FieldSetter.
            var beforeAction = queued.FieldSetter.Action;
            queued.Callback.Invoke(queued.FieldSetter);
            var afterAction = queued.FieldSetter.Action;

            beforeAction.Should().NotBeSameAs(afterAction);
        }

        [Fact]
        public void CreateStaticGetterWorks()
        {
            var field = typeof(Corge).GetField(nameof(Corge.Bar));

            Func<object> getter;
            var queued = (Callback: (WaitCallback)null!, FieldGetter: (StaticFieldGetter)null!);

            void queueUserWorkItem(WaitCallback callback, object state) => queued = (callback, (StaticFieldGetter)state);

            FieldInfoExtensions.SetQueueUserWorkItemAction(queueUserWorkItem);

            try
            {
                getter = field.CreateStaticGetter();
            }
            finally
            {
                FieldInfoExtensions.SetQueueUserWorkItemAction(null!);
            }

            // Verify that a work item was queued
            queued.FieldGetter.Should().NotBeNull();
            queued.Callback.Should().NotBeNull();

            // Verify that the return delegate is correct
            getter.Target.Should().BeSameAs(queued.FieldGetter);
            getter.Method.Name.Should().Be(nameof(StaticFieldGetter.GetValue));
            getter.Method.DeclaringType.Should().Be(typeof(StaticFieldGetter));

            // Verify that the queued Callback calls the SetOptimizedFunc
            // method of the queued FieldGetter.
            var beforeFunc = queued.FieldGetter.Func;
            queued.Callback.Invoke(queued.FieldGetter);
            var afterFunc = queued.FieldGetter.Func;

            beforeFunc.Should().NotBeSameAs(afterFunc);
        }

        [Fact]
        public void CreateStaticGetter1Works()
        {
            var field = typeof(Corge).GetField(nameof(Corge.Bar));

            Func<int> getter;
            var queued = (Callback: (WaitCallback)null!, FieldGetter: (StaticFieldGetter<int>)null!);

            void queueUserWorkItem(WaitCallback callback, object state) => queued = (callback, (StaticFieldGetter<int>)state);

            FieldInfoExtensions.SetQueueUserWorkItemAction(queueUserWorkItem);

            try
            {
                getter = field.CreateStaticGetter<int>();
            }
            finally
            {
                FieldInfoExtensions.SetQueueUserWorkItemAction(null!);
            }

            // Verify that a work item was queued
            queued.FieldGetter.Should().NotBeNull();
            queued.Callback.Should().NotBeNull();

            // Verify that the return delegate is correct
            getter.Target.Should().BeSameAs(queued.FieldGetter);
            getter.Method.Name.Should().Be(nameof(StaticFieldGetter<int>.GetValue));
            getter.Method.DeclaringType.Should().Be(typeof(StaticFieldGetter<int>));

            // Verify that the queued Callback calls the SetOptimizedFunc
            // method of the queued FieldGetter.
            var beforeFunc = queued.FieldGetter.Func;
            queued.Callback.Invoke(queued.FieldGetter);
            var afterFunc = queued.FieldGetter.Func;

            beforeFunc.Should().NotBeSameAs(afterFunc);
        }

        [Fact]
        public void CreateStaticSetterWorks()
        {
            var field = typeof(Corge).GetField(nameof(Corge.Bar));

            Action<object> setter;
            var queued = (Callback: (WaitCallback)null!, FieldSetter: (StaticFieldSetter)null!);

            void queueUserWorkItem(WaitCallback callback, object state) => queued = (callback, (StaticFieldSetter)state);

            FieldInfoExtensions.SetQueueUserWorkItemAction(queueUserWorkItem);

            try
            {
                setter = field.CreateStaticSetter();
            }
            finally
            {
                FieldInfoExtensions.SetQueueUserWorkItemAction(null!);
            }

            // Verify that a work item was queued
            queued.FieldSetter.Should().NotBeNull();
            queued.Callback.Should().NotBeNull();

            // Verify that the return delegate is correct
            setter.Target.Should().BeSameAs(queued.FieldSetter);
            setter.Method.Name.Should().Be(nameof(StaticFieldSetter.SetValue));
            setter.Method.DeclaringType.Should().Be(typeof(StaticFieldSetter));

            // Verify that the queued Callback calls the SetOptimizedFunc
            // method of the queued FieldSetter.
            var beforeAction = queued.FieldSetter.Action;
            queued.Callback.Invoke(queued.FieldSetter);
            var afterAction = queued.FieldSetter.Action;

            beforeAction.Should().NotBeSameAs(afterAction);
        }

        [Fact]
        public void CreateStaticSetter1Works()
        {
            var field = typeof(Corge).GetField(nameof(Corge.Bar));

            Action<int> setter;
            var queued = (Callback: (WaitCallback)null!, FieldSetter: (StaticFieldSetter<int>)null!);

            void queueUserWorkItem(WaitCallback callback, object state) => queued = (callback, (StaticFieldSetter<int>)state);

            FieldInfoExtensions.SetQueueUserWorkItemAction(queueUserWorkItem);

            try
            {
                setter = field.CreateStaticSetter<int>();
            }
            finally
            {
                FieldInfoExtensions.SetQueueUserWorkItemAction(null!);
            }

            // Verify that a work item was queued
            queued.FieldSetter.Should().NotBeNull();
            queued.Callback.Should().NotBeNull();

            // Verify that the return delegate is correct
            setter.Target.Should().BeSameAs(queued.FieldSetter);
            setter.Method.Name.Should().Be(nameof(StaticFieldSetter<int>.SetValue));
            setter.Method.DeclaringType.Should().Be(typeof(StaticFieldSetter<int>));

            // Verify that the queued Callback calls the SetOptimizedFunc
            // method of the queued FieldSetter.
            var beforeAction = queued.FieldSetter.Action;
            queued.Callback.Invoke(queued.FieldSetter);
            var afterAction = queued.FieldSetter.Action;

            beforeAction.Should().NotBeSameAs(afterAction);
        }

#pragma warning disable CA1812 // Used with reflection to test the extensions
        private class Foo
        {
            public int Bar;
            public readonly int Grault;

            public Foo()
            {
                Bar = 0;
                Grault = 0;
            }
        }

        private class Fred
        {
        }
#pragma warning restore CA1812

        private class Corge
        {
            public static int Bar;
#pragma warning disable CS0649 // This is only used to check that a readonly static setter can't be created.  Other rules trigger by fixing this.
            public readonly static int Grault;
#pragma warning restore CS0649

            public Corge()
            {
                Bar = 0;
            }
        }
    }
}
