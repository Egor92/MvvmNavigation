using System;
using System.Windows;
using System.Windows.Controls;
using Egor92.UINavigation.Abstractions;
using Egor92.UINavigation.Wpf.Unity.Internal;
using JetBrains.Annotations;
using Unity;
using Unity.Lifetime;

namespace Egor92.UINavigation.Wpf.Unity
{
    public static class UnityContainerExtensions
    {
        public static void RegisterNavigationManager([NotNull] this IUnityContainer unityContainer, [NotNull] ContentControl frameControl)
        {
            if (unityContainer == null)
                throw new ArgumentNullException(nameof(unityContainer));

            if (frameControl == null)
                throw new ArgumentNullException(nameof(frameControl));

            unityContainer.RegisterInstance(typeof(ContentControl), NavigationManagerCtorInfo.Args.FrameControl.Name, frameControl);
            unityContainer.RegisterType<IDataStorage, UnityDataStorage>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<NavigationManager>(new ContainerControlledLifetimeManager(),
                                                           NavigationManagerCtorInfo.InjectionConstructor);
            var navigationManager = unityContainer.Resolve<NavigationManager>();

            unityContainer.RegisterInstance<INavigationManager>(navigationManager);
        }

        public static void RegisterNavigationRule<TViewModel, TView>([NotNull] this IUnityContainer unityContainer,
                                                                     [NotNull] string navigationKey)
            where TViewModel : class
            where TView : FrameworkElement
        {
            if (unityContainer == null)
                throw new ArgumentNullException(nameof(unityContainer));

            if (navigationKey == null)
                throw new ArgumentNullException(nameof(navigationKey));

            if (typeof(TViewModel).IsInterface)
                throw new ArgumentException(ExceptionMessages.CanNotRegisterInterfaceAsViewModel<TViewModel>());

            if (typeof(TViewModel).IsAbstract)
                throw new ArgumentException(ExceptionMessages.CanNotRegisterAbstractClassAsViewModel<TViewModel>());

            if (typeof(TView).IsAbstract)
                throw new ArgumentException(ExceptionMessages.CanNotRegisterAbstractClassAsView<TView>());

            unityContainer.RegisterType<TViewModel>(navigationKey, new TransientLifetimeManager());
            unityContainer.RegisterType<TView>(navigationKey, new TransientLifetimeManager());
        }
    }
}
