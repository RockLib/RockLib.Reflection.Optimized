using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using Xunit;

namespace RockLib.Reflection.Optimized.Tests
{
    public class PropertyInfoExtensionsTests
    {
        private static readonly PropertyInfo _property = typeof(Foo).GetProperty(nameof(Foo.Bar));

        [Fact]
        public void CreateGetterThrowsIfPropertyParameterIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => PropertyInfoExtensions.CreateGetter(null));
            exception.ParamName.Should().Be("property");
        }

        [Fact]
        public void CreateGetterThrowsIfPropertyParameterHasNoGetter()
        {
            var property = typeof(Foo).GetProperty(nameof(Foo.Baz));
            var exception = Assert.Throws<ArgumentException>(() => property.CreateGetter());
            exception.ParamName.Should().Be("property");
        }

        [Fact]
        public void CreateGetterThrowsIfPropertyParameterHasNonPublicGetter()
        {
            var property = typeof(Foo).GetProperty(nameof(Foo.Qux));
            var exception = Assert.Throws<ArgumentException>(() => property.CreateGetter());
            exception.ParamName.Should().Be("property");
        }

        [Fact]
        public void CreateGetter1ThrowsIfPropertyParameterIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => PropertyInfoExtensions.CreateGetter<int>(null));
            exception.ParamName.Should().Be("property");
        }

        [Fact]
        public void CreateGetter1ThrowsIfTheTypeArgumentIsNotCompatibleWithThePropertyType()
        {
            var property = typeof(Foo).GetProperty(nameof(Foo.Bar));
            var exception = Assert.Throws<ArgumentException>(() => property.CreateGetter<string>());
            exception.ParamName.Should().Be("property");
        }

        [Fact]
        public void CreateGetter1ThrowsIfPropertyParameterHasNoGetter()
        {
            var property = typeof(Foo).GetProperty(nameof(Foo.Baz));
            var exception = Assert.Throws<ArgumentException>(() => property.CreateGetter<int>());
            exception.ParamName.Should().Be("property");
        }

        [Fact]
        public void CreateGetter1ThrowsIfPropertyParameterHasNonPublicGetter()
        {
            var property = typeof(Foo).GetProperty(nameof(Foo.Qux));
            var exception = Assert.Throws<ArgumentException>(() => property.CreateGetter<int>());
            exception.ParamName.Should().Be("property");
        }

        [Fact]
        public void CreateGetter2ThrowsIfPropertyParameterIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => PropertyInfoExtensions.CreateGetter<Foo, int>(null));
            exception.ParamName.Should().Be("property");
        }

        [Fact]
        public void CreateGetter2ThrowsIfTheFirstTypeArgumentIsNotCompatibleWithThePropertyDeclaringType()
        {
            var property = typeof(Foo).GetProperty(nameof(Foo.Bar));
            var exception = Assert.Throws<ArgumentException>(() => property.CreateGetter<Fred, int>());
            exception.ParamName.Should().Be("property");
        }

        [Fact]
        public void CreateGetter2ThrowsIfTheSecondTypeArgumentIsNotCompatibleWithThePropertyType()
        {
            var property = typeof(Foo).GetProperty(nameof(Foo.Bar));
            var exception = Assert.Throws<ArgumentException>(() => property.CreateGetter<Foo, string>());
            exception.ParamName.Should().Be("property");
        }

        [Fact]
        public void CreateGetter2ThrowsIfPropertyParameterHasNoGetter()
        {
            var property = typeof(Foo).GetProperty(nameof(Foo.Baz));
            var exception = Assert.Throws<ArgumentException>(() => property.CreateGetter<Foo, int>());
            exception.ParamName.Should().Be("property");
        }

        [Fact]
        public void CreateGetter2ThrowsIfPropertyParameterHasNonPublicGetter()
        {
            var property = typeof(Foo).GetProperty(nameof(Foo.Qux));
            var exception = Assert.Throws<ArgumentException>(() => property.CreateGetter<Foo, int>());
            exception.ParamName.Should().Be("property");
        }

        [Fact]
        public void CreateSetterThrowsIfPropertyParameterIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => PropertyInfoExtensions.CreateSetter(null));
            exception.ParamName.Should().Be("property");
        }

        [Fact]
        public void CreateSetterThrowsIfPropertyParameterHasNoSetter()
        {
            var property = typeof(Foo).GetProperty(nameof(Foo.Grault));
            var exception = Assert.Throws<ArgumentException>(() => property.CreateSetter());
            exception.ParamName.Should().Be("property");
        }

        [Fact]
        public void CreateSetterThrowsIfPropertyParameterHasNonPublicSetter()
        {
            var property = typeof(Foo).GetProperty(nameof(Foo.Garply));
            var exception = Assert.Throws<ArgumentException>(() => property.CreateSetter());
            exception.ParamName.Should().Be("property");
        }

        [Fact]
        public void CreateSetter1ThrowsIfPropertyParameterIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => PropertyInfoExtensions.CreateSetter<int>(null));
            exception.ParamName.Should().Be("property");
        }

        [Fact]
        public void CreateSetter1ThrowsIfTheTypeArgumentIsNotCompatibleWithThePropertyType()
        {
            var property = typeof(Foo).GetProperty(nameof(Foo.Bar));
            var exception = Assert.Throws<ArgumentException>(() => property.CreateSetter<string>());
            exception.ParamName.Should().Be("property");
        }

        [Fact]
        public void CreateSetter1ThrowsIfPropertyParameterHasNoSetter()
        {
            var property = typeof(Foo).GetProperty(nameof(Foo.Grault));
            var exception = Assert.Throws<ArgumentException>(() => property.CreateSetter<int>());
            exception.ParamName.Should().Be("property");
        }

        [Fact]
        public void CreateSetter1ThrowsIfPropertyParameterHasNonPublicSetter()
        {
            var property = typeof(Foo).GetProperty(nameof(Foo.Garply));
            var exception = Assert.Throws<ArgumentException>(() => property.CreateSetter<int>());
            exception.ParamName.Should().Be("property");
        }

        [Fact]
        public void CreateSetter2ThrowsIfPropertyParameterIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => PropertyInfoExtensions.CreateSetter<Foo, int>(null));
            exception.ParamName.Should().Be("property");
        }

        [Fact]
        public void CreateSetter2ThrowsIfTheFirstTypeArgumentIsNotCompatibleWithThePropertyDeclaringType()
        {
            var property = typeof(Foo).GetProperty(nameof(Foo.Bar));
            var exception = Assert.Throws<ArgumentException>(() => property.CreateSetter<Fred, int>());
            exception.ParamName.Should().Be("property");
        }

        [Fact]
        public void CreateSetter2ThrowsIfTheSecondTypeArgumentIsNotCompatibleWithThePropertyType()
        {
            var property = typeof(Foo).GetProperty(nameof(Foo.Bar));
            var exception = Assert.Throws<ArgumentException>(() => property.CreateSetter<Foo, string>());
            exception.ParamName.Should().Be("property");
        }

        [Fact]
        public void CreateSetter2ThrowsIfPropertyParameterHasNoSetter()
        {
            var property = typeof(Foo).GetProperty(nameof(Foo.Grault));
            var exception = Assert.Throws<ArgumentException>(() => property.CreateSetter<Foo, int>());
            exception.ParamName.Should().Be("property");
        }

        [Fact]
        public void CreateSetter2ThrowsIfPropertyParameterHasNonPublicSetter()
        {
            var property = typeof(Foo).GetProperty(nameof(Foo.Garply));
            var exception = Assert.Throws<ArgumentException>(() => property.CreateSetter<Foo, int>());
            exception.ParamName.Should().Be("property");
        }

        [Fact]
        public void CreateGetterWorks()
        {
            Func<object, object> getter;
            var queued = (Callback: (Action<object>)null, PropertyGetter: (PropertyGetter)null);

            void queueUserWorkItem(Action<object> callback, object state) => queued = (callback, (PropertyGetter)state);

            PropertyInfoExtensions.SetQueueUserWorkItemAction(queueUserWorkItem);

            try
            {
                getter = _property.CreateGetter();
            }
            finally
            {
                PropertyInfoExtensions.SetQueueUserWorkItemAction(null);
            }

            // Verify that a work item was queued
            queued.PropertyGetter.Should().NotBeNull();
            queued.Callback.Should().NotBeNull();

            // Verify that the return delegate is correct
            getter.Target.Should().BeSameAs(queued.PropertyGetter);
            getter.Method.Name.Should().Be(nameof(PropertyGetter.GetValue));
            getter.Method.DeclaringType.Should().Be(typeof(PropertyGetter));

            // Verify that the queued Callback calls the SetOptimizedFunc
            // method of the queued PropertyGetter.
            var beforeFunc = queued.PropertyGetter.Func;
            queued.Callback.Invoke(queued.PropertyGetter);
            var afterFunc = queued.PropertyGetter.Func;

            beforeFunc.Should().NotBeSameAs(afterFunc);
        }

        [Fact]
        public void CreateGetter1Works()
        {
            Func<object, int> getter;
            var queued = (Callback: (Action<object>)null, PropertyGetter: (PropertyGetter<int>)null);

            void queueUserWorkItem(Action<object> callback, object state) => queued = (callback, (PropertyGetter<int>)state);

            PropertyInfoExtensions.SetQueueUserWorkItemAction(queueUserWorkItem);

            try
            {
                getter = _property.CreateGetter<int>();
            }
            finally
            {
                PropertyInfoExtensions.SetQueueUserWorkItemAction(null);
            }

            // Verify that a work item was queued
            queued.PropertyGetter.Should().NotBeNull();
            queued.Callback.Should().NotBeNull();

            // Verify that the return delegate is correct
            getter.Target.Should().BeSameAs(queued.PropertyGetter);
            getter.Method.Name.Should().Be(nameof(PropertyGetter<int>.GetValue));
            getter.Method.DeclaringType.Should().Be(typeof(PropertyGetter<int>));

            // Verify that the queued Callback calls the SetOptimizedFunc
            // method of the queued PropertyGetter.
            var beforeFunc = queued.PropertyGetter.Func;
            queued.Callback.Invoke(queued.PropertyGetter);
            var afterFunc = queued.PropertyGetter.Func;

            beforeFunc.Should().NotBeSameAs(afterFunc);
        }

        [Fact]
        public void CreateGetter2Works()
        {
            Func<Foo, int> getter;
            var queued = (Callback: (Action<object>)null, PropertyGetter: (PropertyGetter<Foo, int>)null);

            void queueUserWorkItem(Action<object> callback, object state) => queued = (callback, (PropertyGetter<Foo, int>)state);

            PropertyInfoExtensions.SetQueueUserWorkItemAction(queueUserWorkItem);

            try
            {
                getter = _property.CreateGetter<Foo, int>();
            }
            finally
            {
                PropertyInfoExtensions.SetQueueUserWorkItemAction(null);
            }

            // Verify that a work item was queued
            queued.PropertyGetter.Should().NotBeNull();
            queued.Callback.Should().NotBeNull();

            // Verify that the return delegate is correct
            getter.Target.Should().BeSameAs(queued.PropertyGetter);
            getter.Method.Name.Should().Be(nameof(PropertyGetter<Foo, int>.GetValue));
            getter.Method.DeclaringType.Should().Be(typeof(PropertyGetter<Foo, int>));

            // Verify that the queued Callback calls the SetOptimizedFunc
            // method of the queued PropertyGetter.
            var beforeFunc = queued.PropertyGetter.Func;
            queued.Callback.Invoke(queued.PropertyGetter);
            var afterFunc = queued.PropertyGetter.Func;

            beforeFunc.Should().NotBeSameAs(afterFunc);
        }

        [Fact]
        public void CreateSetterWorks()
        {
            Action<object, object> setter;
            var queued = (Callback: (Action<object>)null, PropertySetter: (PropertySetter)null);

            void queueUserWorkItem(Action<object> callback, object state) => queued = (callback, (PropertySetter)state);

            PropertyInfoExtensions.SetQueueUserWorkItemAction(queueUserWorkItem);

            try
            {
                setter = _property.CreateSetter();
            }
            finally
            {
                PropertyInfoExtensions.SetQueueUserWorkItemAction(null);
            }

            // Verify that a work item was queued
            queued.PropertySetter.Should().NotBeNull();
            queued.Callback.Should().NotBeNull();

            // Verify that the return delegate is correct
            setter.Target.Should().BeSameAs(queued.PropertySetter);
            setter.Method.Name.Should().Be(nameof(PropertySetter.SetValue));
            setter.Method.DeclaringType.Should().Be(typeof(PropertySetter));

            // Verify that the queued Callback calls the SetOptimizedFunc
            // method of the queued PropertySetter.
            var beforeAction = queued.PropertySetter.Action;
            queued.Callback.Invoke(queued.PropertySetter);
            var afterAction = queued.PropertySetter.Action;

            beforeAction.Should().NotBeSameAs(afterAction);
        }

        [Fact]
        public void CreateSetter1Works()
        {
            Action<object, int> setter;
            var queued = (Callback: (Action<object>)null, PropertySetter: (PropertySetter<int>)null);

            void queueUserWorkItem(Action<object> callback, object state) => queued = (callback, (PropertySetter<int>)state);

            PropertyInfoExtensions.SetQueueUserWorkItemAction(queueUserWorkItem);

            try
            {
                setter = _property.CreateSetter<int>();
            }
            finally
            {
                PropertyInfoExtensions.SetQueueUserWorkItemAction(null);
            }

            // Verify that a work item was queued
            queued.PropertySetter.Should().NotBeNull();
            queued.Callback.Should().NotBeNull();

            // Verify that the return delegate is correct
            setter.Target.Should().BeSameAs(queued.PropertySetter);
            setter.Method.Name.Should().Be(nameof(PropertySetter<int>.SetValue));
            setter.Method.DeclaringType.Should().Be(typeof(PropertySetter<int>));

            // Verify that the queued Callback calls the SetOptimizedFunc
            // method of the queued PropertySetter.
            var beforeAction = queued.PropertySetter.Action;
            queued.Callback.Invoke(queued.PropertySetter);
            var afterAction = queued.PropertySetter.Action;

            beforeAction.Should().NotBeSameAs(afterAction);
        }

        [Fact]
        public void CreateSetter2Works()
        {
            Action<Foo, int> setter;
            var queued = (Callback: (Action<object>)null, PropertySetter: (PropertySetter<Foo, int>)null);

            void queueUserWorkItem(Action<object> callback, object state) => queued = (callback, (PropertySetter<Foo, int>)state);

            PropertyInfoExtensions.SetQueueUserWorkItemAction(queueUserWorkItem);

            try
            {
                setter = _property.CreateSetter<Foo, int>();
            }
            finally
            {
                PropertyInfoExtensions.SetQueueUserWorkItemAction(null);
            }

            // Verify that a work item was queued
            queued.PropertySetter.Should().NotBeNull();
            queued.Callback.Should().NotBeNull();

            // Verify that the return delegate is correct
            setter.Target.Should().BeSameAs(queued.PropertySetter);
            setter.Method.Name.Should().Be(nameof(PropertySetter<Foo, int>.SetValue));
            setter.Method.DeclaringType.Should().Be(typeof(PropertySetter<Foo, int>));

            // Verify that the queued Callback calls the SetOptimizedFunc
            // method of the queued PropertySetter.
            var beforeAction = queued.PropertySetter.Action;
            queued.Callback.Invoke(queued.PropertySetter);
            var afterAction = queued.PropertySetter.Action;

            beforeAction.Should().NotBeSameAs(afterAction);
        }

        public class Foo
        {
            public int Bar { get; set; }
            public int Baz { set { } }
            public int Qux { private get; set; }
            public int Grault => 0;
            public int Garply { get; private set; }
        }

        public class Fred
        {
        }
    }
}
