using System.Threading;
using Avalonia.Controls;
using Egor92.MvvmNavigation.Abstractions;
using Egor92.MvvmNavigation.Avalonia.Unity.IntegrationTests.Internal;
using Egor92.MvvmNavigation.Avalonia.Unity.IntegrationTests.Internal.Types;
using Egor92.MvvmNavigation.Tests.Common.Unity;
using Egor92.MvvmNavigation.Unity;
using NUnit.Framework;
using Unity;

namespace Egor92.MvvmNavigation.Avalonia.Unity.IntegrationTests
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class ScenarioTests
    {
        private UnityContainer _unityContainer;

        [SetUp]
        public void SetUp()
        {
            _unityContainer = new UnityContainer();
        }

        [TearDown]
        public void TearDown()
        {
            _unityContainer.Dispose();
        }

        [Test]
        public void RegisterNavigationManager_NavigationManagerIsRegistered()
        {
            //Arrange
            var frameControl = new ContentControl();

            //Act
            _unityContainer.RegisterNavigationManager(frameControl);

            //Assert
            UnityAssert.IsRegistered<NavigationManager>(_unityContainer);
        }

        [Test]
        public void RegisterNavigationManager_INavigationManagerIsRegistered()
        {
            //Arrange
            var frameControl = new ContentControl();

            //Act
            _unityContainer.RegisterNavigationManager(frameControl);

            //Assert
            UnityAssert.IsRegistered<INavigationManager>(_unityContainer);
        }

        [Test]
        public void RegisterNavigationManager_FrameControlIsNotContentControl_ThrowException()
        {
            //Arrange
            var frameControl = new object();

            //Act
            void Action()
            {
                _unityContainer.RegisterNavigationManager(frameControl);
            }

            //Assert
            Assert.That(Action, ThrowsException.FrameControlIsNotOfFrameControlType(frameControl, typeof(ContentControl)));
        }

        [Test]
        public void RegisterNavigationManager_NavigationManagerCanBeResolved()
        {
            //Arrange
            var frameControl = new ContentControl();

            //Act
            _unityContainer.RegisterNavigationManager(frameControl);

            //Assert
            UnityAssert.CanResolve<NavigationManager>(_unityContainer);
        }

        [Test]
        public void RegisterNavigationRule_CanNavigate()
        {
            //Arrange
            var navigationKey = "navigationKey";
            var frameControl = new ContentControl();
            _unityContainer.RegisterNavigationManager(frameControl);

            //Act
            _unityContainer.RegisterNavigationRule<ViewModel, View>(navigationKey);
            var navigationManager = _unityContainer.Resolve<INavigationManager>();
            navigationManager.Navigate(navigationKey);

            //Assert
            Assert.That(frameControl.Content, Is.TypeOf<View>());
            Assert.That(((View)frameControl.Content).DataContext, Is.TypeOf<ViewModel>());
        }
    }
}
