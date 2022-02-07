using FluentAssertions;
using System;
using Xunit;
using Xunit.Extensions.Ordering;

namespace RockLib.Reflection.Optimized.Tests
{
    [Order(-1)]
    public class UndecorateExtensionsTests
    {
        [Fact]
        public void CanUndecorateClassWithInterfaceField()
        {
            IFoo inner = new ConcreteFoo();
            IFoo foo = new DecoratorFoo1(inner);

            var undecorated = foo.Undecorate();

            undecorated.Should().BeSameAs(inner);
        }

        [Fact]
        public void CanUndecorateMultipleLayers()
        {
            IFoo inner = new ConcreteFoo();
            IFoo foo = new DecoratorFoo1(new DecoratorFoo2 { Foo = new DecoratorFoo1(inner) });

            var undecorated = foo.Undecorate();

            undecorated.Should().BeSameAs(inner);
        }

        [Fact]
        public void CanUndecorateWithNullInnerValue()
        {
            IFoo foo = new DecoratorFoo1(new DecoratorFoo2 { Foo = new DecoratorFoo1(null!) });

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
        public void ReturnsSameObjectIfNoInterfaceFieldExists()
        {
            IFoo foo = new NonDecoratorFoo();

            var undecorated = foo.Undecorate();

            undecorated.Should().BeSameAs(foo);
        }

#pragma warning disable IDE0052 // Remove unread private members

        private interface IFoo
        {
        }

        private class ConcreteFoo : IFoo
        {
        }

        private class DecoratorFoo1 : IFoo
        {
            private readonly IFoo _foo;

            public DecoratorFoo1(IFoo foo) => _foo = foo;
        }

        private class DecoratorFoo2 : IFoo
        {
            public DecoratorFoo2()
            {
                Foo = new ConcreteFoo();
            }

            public IFoo Foo { get; set; }
        }

        private class NonDecoratorFoo : IFoo
        {
        }

        private abstract class AbstractBar
        {
        }

        private class ConcreteBar : AbstractBar
        {
        }

#pragma warning restore IDE0052 // Remove unread private members
    }
}
