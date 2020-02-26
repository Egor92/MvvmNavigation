using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Egor92.MvvmNavigation.Abstractions;
using Egor92.MvvmNavigation.Abstractions.ContractTests;
using Egor92.MvvmNavigation.Core.ContractTests.Internal;
using Egor92.MvvmNavigation.Tests.Common;
using Moq;
using NUnit.Framework;
using ThrowsException = Egor92.MvvmNavigation.Tests.Common.ThrowsException;

namespace Egor92.MvvmNavigation.Core.ContractTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    [Apartment(ApartmentState.STA)]
    [SuppressMessage("ReSharper", "ConvertToLocalFunction")]
    public abstract class NavigationManagerBaseContractTests<TNavigationManager, TContentControl, TView> : NavigationManagerContractTests<TNavigationManager>
        where TNavigationManager : NavigationManagerBase
        where TContentControl : class, new()
        where TView : class, new()
    {
        private TContentControl _frameControl;
        private TView _view;

        public override void SetUp()
        {
            _frameControl = new TContentControl();
            _view = new TView();
            base.SetUp();
        }

        #region Overridden members

        protected sealed override TNavigationManager CreateNavigationManager()
        {
            return CreateNavigationManager(_frameControl);
        }

        #endregion

        #region Abstract members

        protected abstract TNavigationManager CreateNavigationManager(TContentControl frameControl);

        protected abstract TNavigationManager CreateNavigationManager(TContentControl frameControl, IDataStorage dataStorage);

        protected abstract void RegisterNavigationRule(TNavigationManager navigationManager,
                                                       string navigationKey,
                                                       Func<object> viewModelFunc,
                                                       Func<object> viewFunc);

        protected abstract object GetContent(TContentControl contentControl);

        protected abstract object GetDataContext(object view);

        #endregion

        [Test]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void DataStorageIsNull_ThrowException()
        {
            //Act
            TestDelegate action = () =>
            {
                _navigationManager = CreateNavigationManager(_frameControl, null);
            };

            //Assert
            Assert.That(action, ThrowsException.NullArgument(ArgumentNames.DataStorage));
        }

        [Test]
        [TestCase(ApartmentState.MTA)]
        [TestCase(ApartmentState.STA)]
        public async Task CreatedInThreadWithApartmentState_CanRegisterAndNavigate(ApartmentState apartmentState)
        {
            //Arrange
            var navigationKey = "navigationKey";
            var viewModel = new object();
            _navigationManager = await TaskHelper.StartTaskWithApartmentState(apartmentState, CreateNavigationManager);
            RegisterNavigationRule(_navigationManager, navigationKey, () => viewModel, () => _view);

            //Act
            TestDelegate action = () =>
            {
                _navigationManager.Navigate(navigationKey);
            };

            //Assert
            Assert.That(action, Throws.Nothing);
            Assert.That(GetContent(_frameControl), Is.EqualTo(_view));
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
                RegisterNavigationRule(_navigationManager, null, viewModelFunc, viewFunc);
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
                RegisterNavigationRule(_navigationManager, navigationKey, null, viewFunc);
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
                RegisterNavigationRule(_navigationManager, navigationKey, viewModelFunc, null);
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
            RegisterNavigationRule(_navigationManager, navigationKey, viewModelFunc, viewFunc);
            TestDelegate action = () =>
            {
                RegisterNavigationRule(_navigationManager, navigationKey, viewModelFunc, viewFunc);
            };

            //Assert
            Assert.That(action, Internal.ThrowsException.CanNotRegisterKeyTwice());
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
            Assert.That(action, Internal.ThrowsException.KeyIsNotRegistered(navigationKey));
        }

        [Test]
        public void Navigate_KeyIsRegistered_NavigateSuccessfully()
        {
            //Arrange
            var navigationKey = "navigationKey";
            var viewModel = new object();
            RegisterNavigationRule(_navigationManager, navigationKey, () => viewModel, () => _view);

            //Act
            _navigationManager.Navigate(navigationKey);

            //Assert
            var view = GetContent(_frameControl);
            Assert.That(view, Is.EqualTo(_view));
            var dataContext = GetDataContext(view);
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
            RegisterNavigationRule(_navigationManager, navigationKey, () => viewModel.Object, () => _view);

            //Act
            navigate(_navigationManager, navigationKey, navigationArg);

            //Assert
            viewModel.Verify(x => x.OnNavigatedTo(navigationArg), Times.Once);
        }

        [SuppressMessage("ReSharper", "RedundantArgumentDefaultValue")]
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        public static TestCaseData[] NavigatedTo_TestCaseSource()
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
            RegisterNavigationRule(_navigationManager, navigationKey1, () => viewModel.Object, () => _view);
            RegisterNavigationRule(_navigationManager, navigationKey2, () => new object(), () => _view);

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
            RegisterNavigationRule(_navigationManager, navigationKey, () => new object(), () => _view);

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
            RegisterNavigationRule(_navigationManager, navigationKey, () => new object(), () => _view);

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
            RegisterNavigationRule(_navigationManager, navigationKey, () => viewModel, () => _view);

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
