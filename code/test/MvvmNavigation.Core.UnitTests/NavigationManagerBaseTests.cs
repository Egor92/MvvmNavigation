using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Egor92.MvvmNavigation.Core.ContractTests.Internal;
using Moq;
using NUnit.Framework;

namespace Egor92.MvvmNavigation.Core.UnitTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    [Apartment(ApartmentState.STA)]
    public class NavigationManagerBaseTests
    {
        [Test]
        [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
        [SuppressMessage("ReSharper", "UnusedVariable")]
        public void ViewInteractionStrategyIsNull_ThrowException()
        {
            //Arrange
            object frameControl = new object();
            IViewInteractionStrategy viewInteractionStrategy = null;

            //Act
            void Action()
            {
                var navigationManagerBase = new Mock<NavigationManagerBase>(MockBehavior.Strict, frameControl, viewInteractionStrategy).Object;
            }

            //Assert
            var exception = Assert.Catch<Exception>(Action);
            Assert.That(exception.InnerException, Is.TypeOf<ArgumentNullException>());

            var innerException = (ArgumentNullException) exception.InnerException;
            Assert.That(innerException!.ParamName, Is.EqualTo(ArgumentNames.ViewInteractionStrategy));
        }
    }
}
