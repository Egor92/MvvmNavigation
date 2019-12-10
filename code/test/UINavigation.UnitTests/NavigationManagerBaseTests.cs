using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Egor92.UINavigation.ContractTests.Internal;
using Egor92.UINavigation.Tests.Common;
using Moq;
using NUnit.Framework;

namespace Egor92.UINavigation.UnitTests
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
            Assert.That(Action, ThrowsException.InnerException.NullArgument(ArgumentNames.ViewInteractionStrategy));
        }
    }
}
