using FluentAssertions;
using System;
using Xunit;

namespace RockLib.Reflection.Optimized.Tests
{
    public class UndecorateExtensionsTests
    {
        [Fact]
        public void CanUndecorateClassWithInterfaceFieldAndConstructorParameter()
        {
            IFoo inner = new ConcreteFoo();
            IFoo foo = new FieldAndConstructorParameter(inner);

            var undecorated = foo.Undecorate();

            undecorated.Should().BeSameAs(inner);
        }

        [Fact]
        public void CanUndecorateClassWithInterfaceFieldAndPropertySetter()
        {
            IFoo inner = new ConcreteFoo();
            IFoo foo = new FieldAndPropertySetter { Foo = inner };

            var undecorated = foo.Undecorate();

            undecorated.Should().BeSameAs(inner);
        }

        [Fact]
        public void CanUndecorateMultiple()
        {
            IFoo inner = new ConcreteFoo();
            IFoo foo = new FieldAndConstructorParameter(new FieldAndPropertySetter { Foo = new FieldAndConstructorParameter(inner) });

            var undecorated = foo.Undecorate();

            undecorated.Should().BeSameAs(inner);
        }

        [Fact]
        public void CanUndecorateWithNullInnerValue()
        {
            IFoo foo = new FieldAndConstructorParameter(new FieldAndPropertySetter { Foo = new FieldAndConstructorParameter(null) });

            var undecorated = foo.Undecorate();

            undecorated.Should().BeNull();
        }

        [Fact]
        public void ThrowsIfTypeParameterIsNotInterface()
        {
            AbstractBar bar = new ConcreteBar();

            Action act = () => bar.Undecorate();

            act.Should().ThrowExactly<InvalidOperationException>().WithMessage("Generic type argument T must be an interface, but was: *AbstractBar");
        }

        [Fact]
        public void DoesNothingWithInterfaceConstructorParameterButNoInterfaceField()
        {
            IFoo foo = new ConstructorParameterButNoField(new ConcreteFoo());

            var undecorated = foo.Undecorate();

            undecorated.Should().BeSameAs(foo);
        }

        [Fact]
        public void DoesNothingWithInterfacePropertySetterButNoInterfaceField()
        {
            IFoo foo = new PropertySetterButNoField { Foo = new ConcreteFoo() };

            var undecorated = foo.Undecorate();

            undecorated.Should().BeSameAs(foo);
        }

        [Fact]
        public void DoesNothingWithInterfaceFieldButNoPropertySetter()
        {
            IFoo foo = new FieldButNoPropertySetter();

            var undecorated = foo.Undecorate();

            undecorated.Should().BeSameAs(foo);
        }

        [Fact]
        public void DoesNothingWithInterfaceFieldButNonPublicPropertySetter()
        {
            IFoo foo = new FieldButNonPublicPropertySetter();

            var undecorated = foo.Undecorate();

            undecorated.Should().BeSameAs(foo);
        }

#pragma warning disable IDE0052 // Remove unread private members
#pragma warning disable IDE0060 // Remove unused parameter

        public interface IFoo
        {
        }

        public class ConcreteFoo : IFoo
        {
        }

        public class FieldAndConstructorParameter : IFoo
        {
            private readonly IFoo _foo;

            public FieldAndConstructorParameter(IFoo foo) => _foo = foo;
        }

        public class FieldAndPropertySetter : IFoo
        {
            private IFoo _foo;

            public IFoo Foo { set => _foo = value; }
        }

        public class ConstructorParameterButNoField : IFoo
        {
            public ConstructorParameterButNoField(IFoo foo)
            {
            }
        }

        public class PropertySetterButNoField : IFoo
        {
            public IFoo Foo { get => null; set { } }
        }

        public class FieldButNoPropertySetter : IFoo
        {
            public IFoo Foo { get; }
        }

        public class FieldButNonPublicPropertySetter : IFoo
        {
            public IFoo Foo { get; private set; }
        }

        public abstract class AbstractBar
        {
        }

        public class ConcreteBar : AbstractBar
        {
        }

#pragma warning restore IDE0060 // Remove unused parameter
#pragma warning restore IDE0052 // Remove unread private members
    }
}
