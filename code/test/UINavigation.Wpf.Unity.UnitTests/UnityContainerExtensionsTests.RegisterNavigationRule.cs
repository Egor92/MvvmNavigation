using System.Diagnostics.CodeAnalysis;
using System.Windows;
using Egor92.UINavigation.Wpf.Unity.UnitTests.Internal;
using Egor92.UINavigation.Wpf.Unity.UnitTests.Internal.Types;
using NUnit.Framework;
using Unity;

namespace Egor92.UINavigation.Wpf.Unity.UnitTests
{
    [SuppressMessage("ReSharper", "ConvertToLocalFunction")]
    public partial class UnityContainerExtensionsTests
    {
        [Test]
        public void RegisterNavigationRule_UnityContainerIsNull_ThrowException()
        {
            //Arrange
            _unityContainer = null;

            //Act
            TestDelegate action = () =>
            {
                _unityContainer.RegisterNavigationRule<InstanceClass, FrameworkElement>(_navigationKey);
            };

            //Assert
            Assert.That(action, Tests.Common.ThrowsException.NullArgument(ArgumentNames.UnityContainer));
        }

        [Test]
        public void RegisterNavigationRule_ViewModelTypeIsInterface_ThrowException()
        {
            //Act
            TestDelegate action = () =>
            {
                _unityContainer.RegisterNavigationRule<ISomeInterface, FrameworkElement>(_navigationKey);
            };

            //Assert
            Assert.That(action, ThrowsException.CanNotRegisterInterfaceAsViewModel<ISomeInterface>());
        }

        [Test]
        public void RegisterNavigationRule_ViewModelTypeIsAbstractClass_ThrowException()
        {
            //Act
            TestDelegate action = () =>
            {
                _unityContainer.RegisterNavigationRule<AbstractClass, FrameworkElement>(_navigationKey);
            };

            //Assert
            Assert.That(action, ThrowsException.CanNotRegisterAbstractClassAsViewModel<AbstractClass>());
        }

        [Test]
        public void RegisterNavigationRule_ViewTypeIsAbstractClass_ThrowException()
        {
            //Act
            TestDelegate action = () =>
            {
                _unityContainer.RegisterNavigationRule<InstanceClass, AbstractControl>(_navigationKey);
            };

            //Assert
            Assert.That(action, ThrowsException.CanNotRegisterAbstractClassAsView<AbstractControl>());
        }

        [Test]
        public void RegisterNavigationRule_NavigationKeyIsNull_ThrowException()
        {
            //Assert
            _navigationKey = null;

            //Act
            TestDelegate action = () =>
            {
                _unityContainer.RegisterNavigationRule<InstanceClass, AbstractControl>(_navigationKey);
            };

            //Assert
            Assert.That(action, Tests.Common.ThrowsException.NullArgument(ArgumentNames.NavigationKey));
        }

        [Test]
        public void RegisterNavigationRule_ViewTypeIsRegisteredByKeyInUnityContainer()
        {
            //Act
            _unityContainer.RegisterNavigationRule<InstanceClass, FrameworkElement>(_navigationKey);

            //Assert
            Assert.That(_unityContainer.IsRegistered<InstanceClass>(_navigationKey), Is.True);
        }

        [Test]
        public void RegisterNavigationRule_ViewModelTypeIsRegisteredByKeyInUnityContainer()
        {
            //Act
            _unityContainer.RegisterNavigationRule<InstanceClass, FrameworkElement>(_navigationKey);

            //Assert
            Assert.That(_unityContainer.IsRegistered<FrameworkElement>(_navigationKey), Is.True);
        }

        [Test]
        public void RegisterNavigationRule_ViewModelIsRegisteredAsTransient()
        {
            //Act
            _unityContainer.RegisterNavigationRule<InstanceClass, FrameworkElement>(_navigationKey);
            var viewModel1 = _unityContainer.Resolve<InstanceClass>(_navigationKey);
            var viewModel2 = _unityContainer.Resolve<InstanceClass>(_navigationKey);

            //Assert
            Assert.That(viewModel1, Is.Not.EqualTo(viewModel2));
        }

        [Test]
        public void RegisterNavigationRule_ViewIsRegisteredAsTransient()
        {
            //Act
            _unityContainer.RegisterNavigationRule<InstanceClass, FrameworkElement>(_navigationKey);
            var view1 = _unityContainer.Resolve<FrameworkElement>(_navigationKey);
            var view2 = _unityContainer.Resolve<FrameworkElement>(_navigationKey);

            //Assert
            Assert.That(view1, Is.Not.EqualTo(view2));
        }
    }
}
