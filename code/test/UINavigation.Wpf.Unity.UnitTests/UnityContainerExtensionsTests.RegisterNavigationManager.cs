using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Controls;
using Egor92.UINavigation.Abstractions;
using Egor92.UINavigation.Wpf.Unity.UnitTests.Internal;
using NUnit.Framework;
using Unity;
using ThrowsException = Egor92.UINavigation.Tests.Common.ThrowsException;

namespace Egor92.UINavigation.Wpf.Unity.UnitTests
{
    [SuppressMessage("ReSharper", "ConvertToLocalFunction")]
    public partial class UnityContainerExtensionsTests
    {
        [Test]
        public void RegisterNavigationManager_UnityContainerIsNull_ThrowException()
        {
            //Arrange
            _unityContainer = null;

            //Act
            TestDelegate action = () =>
            {
                _unityContainer.RegisterNavigationManager(new ContentControl());
            };

            //Assert
            Assert.That(action, ThrowsException.NullArgument(ArgumentNames.UnityContainer));
        }

        [Test]
        [TestCase(typeof(INavigationManager))]
        [TestCase(typeof(NavigationManager))]
        public void RegisterNavigationManager_NavigationManagerIsRegisteredInContainer(Type navigationManagerType)
        {
            //Act
            _unityContainer.RegisterNavigationManager(new ContentControl());

            //Assert
            var isRegistered = _unityContainer.IsRegistered(navigationManagerType);
            Assert.That(isRegistered, Is.True, $"{navigationManagerType.Name} isn't registered in UnityContainer");
        }

        [Test]
        [TestCase(typeof(INavigationManager))]
        [TestCase(typeof(NavigationManager))]
        public void RegisterNavigationManager_NavigationManagerIsRegisteredAsSingleton(Type navigationManagerType)
        {
            //Act
            _unityContainer.RegisterNavigationManager(new ContentControl());

            //Assert
            var navigationManager1 = _unityContainer.Resolve(navigationManagerType);
            var navigationManager2 = _unityContainer.Resolve(navigationManagerType);
            bool isSingleton = ReferenceEquals(navigationManager1, navigationManager2);
            Assert.That(isSingleton, Is.True, $"{navigationManagerType.Name} isn't registered as singleton");
        }

        [Test]
        public void RegisterNavigationManager_NavigationManagerAndItsInterfaceAreTheSameObjects()
        {
            //Act
            _unityContainer.RegisterNavigationManager(new ContentControl());

            //Assert
            var inavigationManager = _unityContainer.Resolve<INavigationManager>();
            var navigationManager = _unityContainer.Resolve<NavigationManager>();
            bool isSingleton = ReferenceEquals(inavigationManager, navigationManager);
            Assert.That(isSingleton, Is.True, "INavigationManager and NavigationManager aren't the same object");
        }

        [Test]
        public void RegisterNavigationManager_UnityDataStorageIsRegistered()
        {
            //Act
            _unityContainer.RegisterNavigationManager(new ContentControl());

            //Assert
            var dataStorage = _unityContainer.Resolve<IDataStorage>();
            Assert.That(dataStorage, Is.TypeOf<UnityDataStorage>());
        }

        [Test]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void RegisterNavigationManager_FrameControlIsNull_ThrowException()
        {
            //Act
            TestDelegate action = () =>
            {
                _unityContainer.RegisterNavigationManager(null);
            };

            //Assert
            Assert.That(action, ThrowsException.NullArgument(ArgumentNames.FrameControl));
        }
    }
}
