using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Egor92.UINavigation.Abstractions;
using Egor92.UINavigation.Wpf.UnitTests.Internal;
using Moq;
using NUnit.Framework;

namespace Egor92.UINavigation.Wpf.UnitTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    [Apartment(ApartmentState.STA)]
    [SuppressMessage("ReSharper", "ConvertToLocalFunction")]
    public class NavigationManagerTests
    {
        private NavigationManager _navigationManager;
        private ContentControl _frameControl;
        private FrameworkElement _view;

        [SetUp]
        public void SetUp()
        {
            _frameControl = new ContentControl();
            _navigationManager = new NavigationManager(_frameControl);
            _view = Mock.Of<FrameworkElement>();
        }

        [Test]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Ctor_DataStorageIsNull_ThrowException()
        {
            //Act
            TestDelegate action = () =>
            {
                _navigationManager = new NavigationManager(_frameControl, null);
            };

            //Assert
            Assert.That(action, ThrowsException.NullArgument(ArgumentNames.DataStorage));
        }

        [Test]
        [TestCase(ApartmentState.MTA)]
        [TestCase(ApartmentState.STA)]
        public async Task NoDispatcherPassed_CanRegisterAndNavigate(ApartmentState apartmentState)
        {
            //Arrange
            var navigationKey = "navigationKey";
            var viewModel = new object();
            _navigationManager = await TaskHelper.StartTaskWithApartmentState(apartmentState, () => new NavigationManager(_frameControl));
            _navigationManager.Register(navigationKey, () => viewModel, () => _view);

            //Act
            TestDelegate action = () =>
            {
                _navigationManager.Navigate(navigationKey);
            };

            //Assert
            Assert.That(action, Throws.Nothing);
            Assert.That(_frameControl.Content, Is.EqualTo(_view));
        }

        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        [Test]
        public void Register_NavigationKeyIsNull_ThrowException()
        {
            //Arrange
            Func<object> viewModelFunc = () => new object();
            Func<object> viewFunc = () => _view;

            //Act
            TestDelegate action = () =>
            {
                _navigationManager.Register(null, viewModelFunc, viewFunc);
            };

            //Assert
            Assert.That(action, ThrowsException.NullArgument("navigationKey"));
        }

        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        [Test]
        public void Register_ViewModelFuncIsNull_ThrowException()
        {
            //Arrange
            var navigationKey = "navigationKey";
            Func<object> viewFunc = () => _view;

            //Act
            TestDelegate action = () =>
            {
                _navigationManager.Register(navigationKey, null, viewFunc);
            };

            //Assert
            Assert.That(action, ThrowsException.NullArgument(ArgumentNames.GetViewModel));
        }

        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        [Test]
        public void Register_ViewFuncIsNull_ThrowException()
        {
            //Arrange
            var navigationKey = "navigationKey";
            Func<object> viewModelFunc = () => new object();

            //Act
            TestDelegate action = () =>
            {
                _navigationManager.Register(navigationKey, viewModelFunc, null);
            };

            //Assert
            Assert.That(action, ThrowsException.NullArgument(ArgumentNames.GetView));
        }

        [Test]
        public void Register_RegisterNavigationKeyTwice_ThrowException()
        {
            //Arrange
            var navigationKey = "navigationKey";
            Func<object> viewModelFunc = () => new object();
            Func<object> viewFunc = () => _view;

            //Act
            _navigationManager.Register(navigationKey, viewModelFunc, viewFunc);
            TestDelegate action = () =>
            {
                _navigationManager.Register(navigationKey, viewModelFunc, viewFunc);
            };

            //Assert
            Assert.That(action, ThrowsException.CanNotRegisterKeyTwice());
        }

        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        [Test]
        public void Navigate_KeyIsNull_ThrowException()
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
        public void Navigate_NavigateToNotRegisteredKey_ThrowException()
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
        public void Navigate_KeyIsRegistered_NavigateSuccessfully()
        {
            //Arrange
            var navigationKey = "navigationKey";
            var viewModel = new object();
            _navigationManager.Register(navigationKey, () => viewModel, () => _view);

            //Act
            _navigationManager.Navigate(navigationKey);

            //Assert
            Assert.That(_frameControl.Content, Is.EqualTo(_view));
            var dataContext = ((FrameworkElement) _frameControl.Content).DataContext;
            Assert.That(dataContext, Is.EqualTo(viewModel));
        }

        [Test]
        [TestCaseSource(nameof(NavigatedTo_TestCaseSource))]
        public void Navigate_ViewModelIsINavigatedToAware_InvokeNavigatedTo(object navigationArg,
                                                                            Action<INavigationManager, string, object> navigate)
        {
            //Arrange
            var navigationKey = "navigationKey";
            var viewModel = new Mock<INavigatedToAware>();
            _navigationManager.Register(navigationKey, () => viewModel.Object, () => _view);

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
                })).SetArgDisplayNames("arg=null, Navigate(key)"),

                new TestCaseData(null, new Action<INavigationManager, string, object>((navigationManager, key, arg) =>
                {
                    navigationManager.Navigate(key, null);
                })).SetArgDisplayNames("arg=null, Navigate(key, null)"),

                new TestCaseData(new object(), new Action<INavigationManager, string, object>((navigationManager, key, arg) =>
                {
                    navigationManager.Navigate(key, arg);
                })).SetArgDisplayNames("arg=new object(), Navigate(key, arg)"),
            };
        }

        [Test]
        public void Navigate_ViewModelIsINavigatingFromAware_NavigatingFromIsCalled()
        {
            //Arrange
            var navigationKey1 = "navigationKey1";
            var navigationKey2 = "navigationKey2";
            var viewModel = new Mock<INavigatingFromAware>();
            viewModel.Setup(x => x.OnNavigatingFrom())
                     .Verifiable();
            _navigationManager.Register(navigationKey1, () => viewModel.Object, () => _view);
            _navigationManager.Register(navigationKey2, () => new object(), () => _view);

            //Act
            _navigationManager.Navigate(navigationKey1);
            _navigationManager.Navigate(navigationKey2);

            //Assert
            viewModel.Verify(x => x.OnNavigatingFrom(), Times.Once);
        }

        [Test]
        public void Navigate_Navigate_NavigatedIsRaised()
        {
            //Arrange
            var navigationKey = "navigationKey";
            _navigationManager.Register(navigationKey, () => new object(), () => _view);

            bool isNavigatedRaised = false;
            _navigationManager.Navigated += (sender, e) =>
            {
                isNavigatedRaised = true;
            };

            //Act
            _navigationManager.Navigate(navigationKey);

            //Assert
            Assert.That(isNavigatedRaised, Is.True);
        }

        [Test]
        public void Navigated_SenderIsTheNavigationManager()
        {
            //Arrange
            var navigationKey = "navigationKey";
            _navigationManager.Register(navigationKey, () => new object(), () => _view);

            object eventSender = null;
            _navigationManager.Navigated += (sender, e) =>
            {
                eventSender = sender;
            };

            //Act
            _navigationManager.Navigate(navigationKey);

            //Assert
            Assert.That(eventSender, Is.EqualTo(_navigationManager));
        }

        [Test]
        public void Navigated_EventArgsHasAllDesiredValues()
        {
            //Arrange
            var navigationKey = "navigationKey";
            var navigationArg = new object();
            var viewModel = new object();
            _navigationManager.Register(navigationKey, () => viewModel, () => _view);

            NavigationEventArgs eventArgs = null;
            _navigationManager.Navigated += (sender, e) =>
            {
                eventArgs = e;
            };

            //Act
            _navigationManager.Navigate(navigationKey, navigationArg);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(eventArgs, Is.Not.Null);
                Assert.That(eventArgs.View, Is.EqualTo(_view));
                Assert.That(eventArgs.ViewModel, Is.EqualTo(viewModel));
                Assert.That(eventArgs.NavigationKey, Is.EqualTo(navigationKey));
                Assert.That(eventArgs.NavigationArg, Is.EqualTo(navigationArg));
            });
        }
    }
}
