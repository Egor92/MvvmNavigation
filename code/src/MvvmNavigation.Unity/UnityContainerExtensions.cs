using System;
using Egor92.MvvmNavigation.Abstractions;
using Egor92.MvvmNavigation.Internal;
using Egor92.MvvmNavigation.Unity.Internal;
using JetBrains.Annotations;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using ExceptionMessages = Egor92.MvvmNavigation.Unity.Internal.ExceptionMessages;

namespace Egor92.MvvmNavigation.Unity
{
    public static class UnityContainerExtensions
    {
        public static void RegisterNavigationManager([NotNull] this IUnityContainer unityContainer, [NotNull] object frameControl)
        {
            unityContainer.RegisterNavigationManager(frameControl, new NavigationManagerTypeProvider(), new FrameControlTypeProvider());
        }

        internal static void RegisterNavigationManager([NotNull] this IUnityContainer unityContainer,
                                                       [NotNull] object frameControl,
                                                       [NotNull] INavigationManagerTypeProvider navigationManagerTypeProvider,
                                                       [NotNull] IFrameControlTypeProvider frameControlTypeProvider)
        {
            if (navigationManagerTypeProvider == null)
                throw new ArgumentNullException(nameof(navigationManagerTypeProvider));

            if (frameControlTypeProvider == null)
                throw new ArgumentNullException(nameof(frameControlTypeProvider));

            var navigationManagerType = navigationManagerTypeProvider.GetNavigationManagerType();
            if (navigationManagerType == null)
                throw new InvalidOperationException(ExceptionMessages.NavigationManagerTypeIsNotFound());

            var frameControlType = frameControlTypeProvider.GetFrameControlType(navigationManagerType);
            if (frameControlType == null)
                throw new InvalidOperationException(ExceptionMessages.FrameControlTypeIsNotFound());

            unityContainer.RegisterInstance(frameControlType, NavigationManagerCtorInfo.Args.FrameControl.Name, frameControl);
            var injectionMembers = new InjectionMember[]
            {
                NavigationManagerCtorInfo.GetInjectionConstructor(frameControlType)
            };

            unityContainer.RegisterNavigationManager(frameControl, navigationManagerType, frameControlType, injectionMembers);
        }

        public static void RegisterNavigationManager<TNavigationManager, TFrameControl>([NotNull] this IUnityContainer unityContainer,
                                                                                        [NotNull] object frameControl,
                                                                                        [NotNull] InjectionMember[] injectionMembers)
            where TNavigationManager : INavigationManager
        {
            unityContainer.RegisterNavigationManager(frameControl, typeof(TNavigationManager), typeof(TFrameControl), injectionMembers);
        }

        internal static void RegisterNavigationManager([NotNull] this IUnityContainer unityContainer,
                                                       [NotNull] object frameControl,
                                                       [NotNull] Type navigationManagerType,
                                                       [NotNull] Type frameControlType,
                                                       [NotNull] InjectionMember[] injectionMembers)
        {
            if (unityContainer == null)
                throw new ArgumentNullException(nameof(unityContainer));

            if (frameControl == null)
                throw new ArgumentNullException(nameof(frameControl));

            if (navigationManagerType == null)
                throw new ArgumentNullException(nameof(navigationManagerType));

            if (navigationManagerType.IsAbstract)
                throw new ArgumentException(ExceptionMessages.CanNotRegisterAbstractNavigationManagerType(navigationManagerType));

            if (!typeof(INavigationManager).IsAssignableFrom(navigationManagerType))
                throw new ArgumentException(ExceptionMessages.CanNotRegisterTypeAsNavigationManager(navigationManagerType));

            if (frameControlType == null)
                throw new ArgumentNullException(nameof(frameControlType));

            if (!frameControlType.IsInstanceOfType(frameControl))
                throw new ArgumentException(ExceptionMessages.FrameControlIsNotOfFrameControlType(frameControl, frameControlType));

            if (injectionMembers == null)
                throw new ArgumentNullException(nameof(injectionMembers));

            unityContainer.RegisterType<IDataStorage, UnityDataStorage>(new ContainerControlledLifetimeManager());
            try
            {
                unityContainer.RegisterType(navigationManagerType, new ContainerControlledLifetimeManager(), injectionMembers);
            }
            catch (InvalidOperationException e)
            {
                throw new ArgumentException(ExceptionMessages.FailedToRegisterNavigationManager(), e);
            }

            var navigationManager = (INavigationManager) unityContainer.Resolve(navigationManagerType);

            unityContainer.RegisterInstance(navigationManager);
        }

        public static void RegisterNavigationRule<TViewModel, TView>([NotNull] this IUnityContainer unityContainer,
                                                                     [NotNull] string navigationKey)
            where TViewModel : class
            where TView : class
        {
            if (unityContainer == null)
                throw new ArgumentNullException(nameof(unityContainer));

            if (navigationKey == null)
                throw new ArgumentNullException(nameof(navigationKey));

            if (typeof(TViewModel).IsInterface)
                throw new ArgumentException(ExceptionMessages.CanNotRegisterInterfaceAsViewModel<TViewModel>());

            if (typeof(TViewModel).IsAbstract)
                throw new ArgumentException(ExceptionMessages.CanNotRegisterAbstractClassAsViewModel<TViewModel>());

            if (typeof(TView).IsInterface)
                throw new ArgumentException(ExceptionMessages.CanNotRegisterInterfaceAsView<TView>());

            if (typeof(TView).IsAbstract)
                throw new ArgumentException(ExceptionMessages.CanNotRegisterAbstractClassAsView<TView>());

            var navigationData = new RegistrationData(ViewModelFunc, ViewFunc);
            unityContainer.RegisterInstance(navigationKey, navigationData, new ContainerControlledLifetimeManager());

            object ViewModelFunc()
            {
                return unityContainer.Resolve<TViewModel>();
            }

            object ViewFunc()
            {
                return unityContainer.Resolve<TView>();
            }
        }
    }
}
