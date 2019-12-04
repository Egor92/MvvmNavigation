using System;
using Egor92.UINavigation.Abstractions;

namespace Egor92.UINavigation.Unity.Internal
{
    internal static class ExceptionMessages
    {
        internal static string CanNotRegisterInterfaceAsViewModel<TViewModel>()
        {
            return $"Can't register interface {typeof(TViewModel).FullName} as ViewModel. Type of ViewModel must be instance class";
        }

        internal static string CanNotRegisterAbstractClassAsViewModel<TViewModel>()
        {
            return $"Can't register abstract class {typeof(TViewModel).FullName} as ViewModel. Type of ViewModel must be instance class";
        }

        internal static string CanNotRegisterInterfaceAsView<TView>()
        {
            return $"Can't register interface {typeof(TView).FullName} as View. Type of View must be instance class";
        }

        internal static string CanNotRegisterAbstractClassAsView<TView>()
        {
            return $"Can't register abstract class {typeof(TView).FullName} as View. Type of View must be instance class";
        }

        internal static string CanNotRegisterTypeAsNavigationManager(Type navigationManagerType)
        {
            return
                $"Can't register type {navigationManagerType.FullName} as NavigationManager. NavigationManager type must implement {typeof(INavigationManager).FullName} interface";
        }

        internal static string FailedToRegisterNavigationManager()
        {
            return "It's failed to register NavigationManager";
        }

        internal static string CanNotRegisterAbstractNavigationManagerType(Type navigationManagerType)
        {
            return $"Can't register abstract NavigationManager type {navigationManagerType.FullName}";
        }

        internal static string FrameControlIsNotOfFrameControlType(object frameControl, Type frameControlType)
        {
            return $"FrameControl ({frameControl.GetType().FullName}) is not of FrameControlType ({frameControlType.FullName})";
        }

        internal static string FrameControlTypeIsNotFound()
        {
            return "FrameControlType isn't found";
        }

        internal static string NavigationManagerTypeIsNotFound()
        {
            return "NavigationManagerType isn't found";
        }
    }
}
