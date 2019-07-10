using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Egor92.UINavigation.Abstractions;
using Egor92.UINavigation.Wpf.Tests.Internal;
using Moq;
using NUnit.Framework;

namespace Egor92.UINavigation.Wpf.Tests
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    [SuppressMessage("ReSharper", "ConvertToLocalFunction")]
    public class NavigationManagerTests
    {
        private NavigationManager _navigationManager;
        private ContentControl _frameControl;

        [SetUp]
        public void SetUp()
        {
            _frameControl = new ContentControl();
            _navigationManager = new NavigationManager(_frameControl);
        }

        [Test]
        [TestCase(ApartmentState.MTA)]
        [TestCase(ApartmentState.STA)]
        public async Task WhenNoDispatcherPassed_ThenCanRegisterAndNavigate(ApartmentState apartmentState)
        {
            //Arrange
            var navigationKey = "navigationKey";
            var viewModel = new object();
            _navigationManager = await TaskHelper.StartTaskWithApartmentState(apartmentState, () => new NavigationManager(_frameControl));

            //Act
            TestDelegate action = () =>
            {
                _navigationManager.Register<object, TestControl>(navigationKey, () => viewModel);
                _navigationManager.Navigate(navigationKey);
            };

            //Assert
            Assert.That(action, Throws.Nothing);
            Assert.That(_frameControl.Content, Is.TypeOf<TestControl>());
        }

        [Test]
        public void Register_WhenNavigationKeyIsNull_ThenThrowException()
        {
            //Arrange
            Func<object> viewModelFunc = () => new object();

            //Act
            TestDelegate action = () =>
            {
                _navigationManager.Register<object, ContentControl>(null, viewModelFunc);
            };

            //Assert
            Assert.That(action, ThrowsException.NullArgument("navigationKey"));
        }

        [Test]
        public void Register_WhenViewModelFuncIsNull_ThenThrowException()
        {
            //Arrange
            var navigationKey = "navigationKey";

            //Act
            TestDelegate action = () =>
            {
                _navigationManager.Register<object, ContentControl>(navigationKey, null);
            };

            //Assert
            Assert.That(action, ThrowsException.NullArgument(ArgumentNames.ViewModelFunc));
        }

        [Test]
        public void Register_WhenRegisterNavigationKeyTwice_ThenThrowException()
        {
            //Arrange
            var navigationKey = "navigationKey";
            Func<object> viewModelFunc = () => new object();

            //Act
            _navigationManager.Register<object, ContentControl>(navigationKey, viewModelFunc);
            TestDelegate action = () =>
            {
                _navigationManager.Register<object, ContentControl>(navigationKey, viewModelFunc);
            };

            //Assert
            Assert.That(action, ThrowsException.CanNotRegisterKeyTwice());
        }

        [Test]
        public void Navigate_WhenKeyIsNull_ThenThrowException()
        {
            //Arrange

            //Act
            TestDelegate action = () =>
            {
                _navigationManager.Navigate(null);
            };

            //Assert
            Assert.That(action, ThrowsException.NullArgument(ArgumentNames.NavigationKey));
        }

        [Test]
        public void Navigate_WhenNavigateToNotRegisteredKey_ThenThrowException()
        {
            //Arrange
            var navigationKey = "navigationKey";
            Func<object> viewModelFunc = () => new object();

            //Act
            TestDelegate action = () =>
            {
                _navigationManager.Navigate(navigationKey, viewModelFunc);
            };

            //Assert
            Assert.That(action, ThrowsException.KeyIsNotRegistered(navigationKey));
        }

        [Test]
        public void WhenNavigateToRegisteredKey_ThenNavigateSuccessfully()
        {
            //Arrange
            var navigationKey = "navigationKey";
            var viewModel = new object();
            _navigationManager.Register<object, TestControl>(navigationKey, () => viewModel);

            //Act
            _navigationManager.Navigate(navigationKey);

            //Assert
            Assert.That(_frameControl.Content, Is.TypeOf<TestControl>());
            var dataContext = ((FrameworkElement) _frameControl.Content).DataContext;
            Assert.That(dataContext, Is.EqualTo(viewModel));
        }

        [Test]
        [TestCaseSource(nameof(NavigatedTo_TestCaseSource))]
        public void Navigate_WhenViewModelIsINavigatedToAware_ThenNavigatedToIsCalled(object navigationArg,
                                                                                      Action<INavigationManager, string, object> navigate)
        {
            //Arrange
            var navigationKey = "navigationKey";
            var viewModel = new Mock<INavigatedToAware>();
            viewModel.Setup(x => x.OnNavigatedTo(navigationArg))
                     .Verifiable();
            _navigationManager.Register<object, TestControl>(navigationKey, () => viewModel.Object);

            //Act
            navigate(_navigationManager, navigationKey, navigationArg);

            //Assert
            viewModel.Verify(x => x.OnNavigatedTo(navigationArg), Times.Once);
        }

        [SuppressMessage("ReSharper", "RedundantArgumentDefaultValue")]
        private static TestCaseData[] NavigatedTo_TestCaseSource()
        {
            return new[]
            {
                new TestCaseData(null, new Action<INavigationManager, string, object>((navigationManager, key, arg) =>
                {
                    navigationManager.Navigate(key);
                })),
                new TestCaseData(null, new Action<INavigationManager, string, object>((navigationManager, key, arg) =>
                {
                    navigationManager.Navigate(key, null);
                })),
                new TestCaseData(new object(), new Action<INavigationManager, string, object>((navigationManager, key, arg) =>
                {
                    navigationManager.Navigate(key, arg);
                })),
            };
        }

        [Test]
        public void Navigate_WhenViewModelIsINavigatingFromAware_ThenNavigatingFromIsCalled()
        {
            //Arrange
            var navigationKey1 = "navigationKey1";
            var navigationKey2 = "navigationKey2";
            var viewModel = new Mock<INavigatingFromAware>();
            viewModel.Setup(x => x.OnNavigatingFrom())
                     .Verifiable();
            _navigationManager.Register<object, TestControl>(navigationKey1, () => viewModel.Object);
            _navigationManager.Register<object, TestControl>(navigationKey2, () => new object());

            //Act
            _navigationManager.Navigate(navigationKey1);
            _navigationManager.Navigate(navigationKey2);

            //Assert
            viewModel.Verify(x => x.OnNavigatingFrom(), Times.Once);
        }
    }
}
