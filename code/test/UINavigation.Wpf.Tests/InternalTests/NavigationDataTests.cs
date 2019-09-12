using System.Diagnostics.CodeAnalysis;
using Egor92.UINavigation.Wpf.Internal;
using Egor92.UINavigation.Wpf.Tests.Internal;
using NUnit.Framework;

namespace Egor92.UINavigation.Wpf.Tests.InternalTests
{
    [TestFixture]
    internal class NavigationDataTests
    {
        [Test]
        [SuppressMessage("ReSharper", "ConvertToLocalFunction")]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void ViewModelFuncIsNull_ThrowException()
        {
            //Act
            TestDelegate action = () =>
            {
                new NavigationData(null, () => new object());
            };

            //Assert
            Assert.That(action, ThrowsException.NullArgument(ArgumentNames.ViewModelFunc));
        }

        [Test]
        [SuppressMessage("ReSharper", "ConvertToLocalFunction")]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void ViewFuncIsNull_ThrowException()
        {
            //Act
            TestDelegate action = () =>
            {
                new NavigationData(() => new object(), null);
            };

            //Assert
            Assert.That(action, ThrowsException.NullArgument(ArgumentNames.ViewFunc));
        }
    }
}
