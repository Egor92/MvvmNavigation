using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Egor92.UINavigation.Abstractions;
using Egor92.UINavigation.Internal;
using Egor92.UINavigation.Tests.Common.Unity;
using Egor92.UINavigation.Unity.UnitTests.Internal;
using Egor92.UINavigation.Unity.UnitTests.Internal.Types;
using Moq;
using NUnit.Framework;
using Unity;
using Unity.Injection;
using ThrowsException = Egor92.UINavigation.Tests.Common.ThrowsException;

namespace Egor92.UINavigation.Unity.UnitTests
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    [Parallelizable(ParallelScope.Children)]
    public class UnityContainerExtensionsTests
    {
        private IUnityContainer _unityContainer;
        private string _navigationKey;
        private object _frameControl;
        private Type _navigationManagerType;
        private Type _frameControlType;
        private InjectionMember[] _injectionMembers;

        [SetUp]
        public void SetUp()
        {
            _unityContainer = new UnityContainer();
            _navigationKey = TestContext.CurrentContext.Random.GetString();
            _frameControl = new ContentControl();
            _navigationManagerType = typeof(NavigationManager);
            _frameControlType = typeof(ContentControl);
            _injectionMembers = new InjectionMember[0];
        }

        [TearDown]
        public void TearDown()
        {
            _unityContainer?.Dispose();
        }

        #region RegisterNavigationManager

        [Test]
        public void RegisterNavigationManager_UnityContainerIsNull_ThrowException()
        {
            //Arrange
            _unityContainer = null;

            //Act
            void Action()
            {
                _unityContainer.RegisterNavigationManager(_frameControl, _navigationManagerType, _frameControlType, _injectionMembers);
            }

            //Assert
            Assert.That(Action, ThrowsException.NullArgument(ArgumentNames.UnityContainer));
        }

        [Test]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void RegisterNavigationManager_FrameControlIsNull_ThrowException()
        {
            //Act
            void Action()
            {
                _unityContainer.RegisterNavigationManager(null, _navigationManagerType, _frameControlType, _injectionMembers);
            }

            //Assert
            Assert.That(Action, ThrowsException.NullArgument(ArgumentNames.FrameControl));
        }

        [Test]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void RegisterNavigationManager_NavigationManagerTypeIsNull_ThrowException()
        {
            //Act
            void Action()
            {
                _unityContainer.RegisterNavigationManager(_frameControl, null, _frameControlType, _injectionMembers);
            }

            //Assert
            Assert.That(Action, ThrowsException.NullArgument(ArgumentNames.NavigationManagerType));
        }

        [Test]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void RegisterNavigationManager_FrameControlTypeIsNull_ThrowException()
        {
            //Act
            void Action()
            {
                _unityContainer.RegisterNavigationManager(_frameControl, _navigationManagerType, null, _injectionMembers);
            }

            //Assert
            Assert.That(Action, ThrowsException.NullArgument(ArgumentNames.FrameControlType));
        }

        [Test]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void RegisterNavigationManager_InjectionMembersIsNull_ThrowException()
        {
            //Act
            void Action()
            {
                _unityContainer.RegisterNavigationManager(_frameControl, _navigationManagerType, _frameControlType, null);
            }

            //Assert
            Assert.That(Action, ThrowsException.NullArgument(ArgumentNames.InjectionMembers));
        }

        [Test]
        public void RegisterNavigationManager_NavigationManagerIsRegisteredInContainer()
        {
            //Act
            _unityContainer.RegisterNavigationManager(_frameControl, _navigationManagerType, _frameControlType, _injectionMembers);

            //Assert
            UnityAssert.IsRegistered<INavigationManager>(_unityContainer);
        }

        [Test]
        public void RegisterNavigationManager_NavigationManagerTypeDoesNotImplementINavigationManager_ThrowException()
        {
            //Act
            void Action()
            {
                _unityContainer.RegisterNavigationManager(_frameControl, typeof(object), _frameControlType, _injectionMembers);
            }

            //Assert
            Assert.That(Action, Internal.ThrowsException.CanNotRegisterTypeAsNavigationManager(typeof(object)));
        }

        [Test]
        [TestCase(typeof(INavigationManager))]
        [TestCase(typeof(NavigationManager))]
        public void RegisterNavigationManager_NavigationManagerIsRegisteredAsSingleton(Type navigationManagerType)
        {
            //Act
            _unityContainer.RegisterNavigationManager(_frameControl, typeof(NavigationManager), _frameControlType, _injectionMembers);

            //Assert
            var navigationManager1 = _unityContainer.Resolve(navigationManagerType);
            var navigationManager2 = _unityContainer.Resolve(navigationManagerType);
            bool isSingleton = ReferenceEquals(navigationManager1, navigationManager2);
            Assert.That(isSingleton, Is.True, $"{navigationManagerType.Name} isn't registered as singleton");
        }

        [Test]
        public void RegisterNavigationManager_NavigationManagerAndItsInterfaceAreRegisteredAsSingleton()
        {
            //Act
            _unityContainer.RegisterNavigationManager(_frameControl, typeof(NavigationManager), _frameControlType, _injectionMembers);

            //Assert
            var navigationManager1 = _unityContainer.Resolve<NavigationManager>();
            var navigationManager2 = _unityContainer.Resolve<INavigationManager>();
            bool isSingleton = ReferenceEquals(navigationManager1, navigationManager2);
            Assert.That(isSingleton, Is.True);
        }

        [Test]
        public void RegisterNavigationManager_UnityDataStorageIsRegistered()
        {
            //Act
            _unityContainer.RegisterNavigationManager(_frameControl, _navigationManagerType, _frameControlType, _injectionMembers);

            //Assert
            var dataStorage = _unityContainer.Resolve<IDataStorage>();
            Assert.That(dataStorage, Is.TypeOf<UnityDataStorage>());
        }

        [Test]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void RegisterNavigationManager_NavigationManagerTypeIsRegistered()
        {
            //Act
            _unityContainer.RegisterNavigationManager(_frameControl, _navigationManagerType, _frameControlType, _injectionMembers);

            //Assert
            UnityAssert.IsRegistered<NavigationManager>(_unityContainer);
            UnityAssert.IsRegistered<INavigationManager>(_unityContainer);
        }

        [Test]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void RegisterNavigationManager_NavigationManagerTypeIsAbstract_ThrowException()
        {
            //Act
            void Action()
            {
                _unityContainer.RegisterNavigationManager(_frameControl, typeof(NavigationManagerBase), _frameControlType, _injectionMembers);
            }

            //Assert
            Assert.That(Action, Internal.ThrowsException.CanNotRegisterAbstractNavigationManagerType(typeof(NavigationManagerBase)));
        }

        [Test]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void RegisterNavigationManager_FrameControlIsNotOfFrameControlType_ThrowException()
        {
            //Arrange
            var frameControl = new object();
            var frameControlType = typeof(ContentControl);

            //Act
            void Action()
            {
                _unityContainer.RegisterNavigationManager(frameControl, _navigationManagerType, frameControlType, _injectionMembers);
            }

            //Assert
            Assert.That(Action, Internal.ThrowsException.FrameControlIsNotOfFrameControlType(frameControl, frameControlType));
        }

        [Test]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void RegisterNavigationManager_NavigationManagerTypeProviderIsNull_ThrowException()
        {
            //Arrange
            var frameControlTypeProvider = Mock.Of<IFrameControlTypeProvider>();

            //Act
            void Action()
            {
                _unityContainer.RegisterNavigationManager(_frameControl, null, frameControlTypeProvider);
            }

            //Assert
            Assert.That(Action, ThrowsException.NullArgument(ArgumentNames.NavigationManagerTypeProvider));
        }

        [Test]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void RegisterNavigationManager_FrameControlTypeProviderIsNull_ThrowException()
        {
            //Arrange
            var navigationManagerTypeProvider = Mock.Of<INavigationManagerTypeProvider>();

            //Act
            void Action()
            {
                _unityContainer.RegisterNavigationManager(_frameControl, navigationManagerTypeProvider, null);
            }

            //Assert
            Assert.That(Action, ThrowsException.NullArgument(ArgumentNames.FrameControlTypeProvider));
        }

        [Test]
        public void RegisterNavigationManager_NavigationManagerTypeProviderReturnsNull_ThrowException()
        {
            //Arrange
            var navigationManagerTypeProvider = Mock.Of<INavigationManagerTypeProvider>(x => x.GetNavigationManagerType() == null);
            var frameControlTypeProvider = Mock.Of<IFrameControlTypeProvider>();

            //Act
            void Action()
            {
                _unityContainer.RegisterNavigationManager(_frameControl, navigationManagerTypeProvider, frameControlTypeProvider);
            }

            //Assert
            Assert.That(Action, Internal.ThrowsException.NavigationManagerTypeIsNotFound());
        }

        [Test]
        public void RegisterNavigationManager_FrameControlTypeProviderReturnsNull_ThrowException()
        {
            //Arrange
            var navigationManagerTypeProvider = Mock.Of<INavigationManagerTypeProvider>(x => x.GetNavigationManagerType() == typeof(object));
            var frameControlTypeProvider = Mock.Of<IFrameControlTypeProvider>(x => x.GetFrameControlType(typeof(object)) == null);

            //Act
            void Action()
            {
                _unityContainer.RegisterNavigationManager(_frameControl, navigationManagerTypeProvider, frameControlTypeProvider);
            }

            //Assert
            Assert.That(Action, Internal.ThrowsException.FrameControlTypeIsNotFound());
        }

        #endregion

        #region RegisterNavigationRule

        [Test]
        public void RegisterNavigationRule_UnityContainerIsNull_ThrowException()
        {
            //Arrange
            _unityContainer = null;

            //Act
            void Action()
            {
                _unityContainer.RegisterNavigationRule<InstanceClass, object>(_navigationKey);
            }

            //Assert
            Assert.That(Action, ThrowsException.NullArgument(ArgumentNames.UnityContainer));
        }

        [Test]
        public void RegisterNavigationRule_ViewModelTypeIsInterface_ThrowException()
        {
            //Act
            void Action()
            {
                _unityContainer.RegisterNavigationRule<ISomeInterface, object>(_navigationKey);
            }

            //Assert
            Assert.That(Action, Internal.ThrowsException.CanNotRegisterInterfaceAsViewModel<ISomeInterface>());
        }

        [Test]
        public void RegisterNavigationRule_ViewModelTypeIsAbstractClass_ThrowException()
        {
            //Act
            void Action()
            {
                _unityContainer.RegisterNavigationRule<AbstractClass, object>(_navigationKey);
            }

            //Assert
            Assert.That(Action, Internal.ThrowsException.CanNotRegisterAbstractClassAsViewModel<AbstractClass>());
        }

        [Test]
        public void RegisterNavigationRule_ViewTypeIsInterface_ThrowException()
        {
            //Act
            void Action()
            {
                _unityContainer.RegisterNavigationRule<object, ISomeInterface>(_navigationKey);
            }

            //Assert
            Assert.That(Action, Internal.ThrowsException.CanNotRegisterInterfaceAsView<ISomeInterface>());
        }

        [Test]
        public void RegisterNavigationRule_ViewTypeIsAbstractClass_ThrowException()
        {
            //Act
            void Action()
            {
                _unityContainer.RegisterNavigationRule<object, AbstractClass>(_navigationKey);
            }

            //Assert
            Assert.That(Action, Internal.ThrowsException.CanNotRegisterAbstractClassAsView<AbstractClass>());
        }

        [Test]
        public void RegisterNavigationRule_NavigationKeyIsNull_ThrowException()
        {
            //Assert
            _navigationKey = null;

            //Act
            void Action()
            {
                _unityContainer.RegisterNavigationRule<object, object>(_navigationKey);
            }

            //Assert
            Assert.That(Action, ThrowsException.NullArgument(ArgumentNames.NavigationKey));
        }

        [Test]
        public void RegisterNavigationRule_NavigationDataIsRegisteredByKeyInUnityContainer()
        {
            //Act
            _unityContainer.RegisterNavigationRule<InstanceClass, InstanceClass>(_navigationKey);

            //Assert
            UnityAssert.IsRegisteredWithName<NavigationData>(_unityContainer, _navigationKey);
        }

        [Test]
        public void RegisterNavigationRule_ViewModelIsRegisteredAsTransient()
        {
            //Act
            _unityContainer.RegisterNavigationRule<InstanceClass, object>(_navigationKey);

            var navigationData1 = _unityContainer.Resolve<NavigationData>(_navigationKey);
            var viewModel1 = navigationData1.ViewModelFunc();

            var navigationData2 = _unityContainer.Resolve<NavigationData>(_navigationKey);
            var viewModel2 = navigationData2.ViewModelFunc();

            //Assert
            Assert.That(viewModel1, Is.Not.EqualTo(viewModel2));
        }

        [Test]
        public void RegisterNavigationRule_ViewIsRegisteredAsTransient()
        {
            //Act
            _unityContainer.RegisterNavigationRule<object, InstanceClass>(_navigationKey);

            var navigationData1 = _unityContainer.Resolve<NavigationData>(_navigationKey);
            var view1 = navigationData1.ViewFunc();

            var navigationData2 = _unityContainer.Resolve<NavigationData>(_navigationKey);
            var view2 = navigationData2.ViewFunc();

            //Assert
            Assert.That(view1, Is.Not.EqualTo(view2));
        }

        #endregion
    }
}
