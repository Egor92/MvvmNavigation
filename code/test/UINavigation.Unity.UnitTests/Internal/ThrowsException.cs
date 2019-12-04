using System;
using Egor92.UINavigation.Unity.Internal;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Egor92.UINavigation.Unity.UnitTests.Internal
{
    internal static class ThrowsException
    {
        internal static IResolveConstraint CanNotRegisterInterfaceAsViewModel<TViewModel>()
        {
            var message = ExceptionMessages.CanNotRegisterInterfaceAsViewModel<TViewModel>();
            return Throws.ArgumentException.And.Message.EqualTo(message);
        }

        internal static IResolveConstraint CanNotRegisterAbstractClassAsViewModel<TViewModel>()
        {
            var message = ExceptionMessages.CanNotRegisterAbstractClassAsViewModel<TViewModel>();
            return Throws.ArgumentException.And.Message.EqualTo(message);
        }

        internal static IResolveConstraint CanNotRegisterInterfaceAsView<TView>()
        {
            var message = ExceptionMessages.CanNotRegisterInterfaceAsView<TView>();
            return Throws.ArgumentException.And.Message.EqualTo(message);
        }

        internal static IResolveConstraint CanNotRegisterAbstractClassAsView<TView>()
        {
            var message = ExceptionMessages.CanNotRegisterAbstractClassAsView<TView>();
            return Throws.ArgumentException.And.Message.EqualTo(message);
        }

        internal static IResolveConstraint CanNotRegisterTypeAsNavigationManager(Type navigationManagerType)
        {
            var message = ExceptionMessages.CanNotRegisterTypeAsNavigationManager(navigationManagerType);
            return Throws.ArgumentException.And.Message.EqualTo(message);
        }

        internal static IResolveConstraint FailedToRegisterNavigationManager()
        {
            var message = ExceptionMessages.FailedToRegisterNavigationManager();
            return Throws.ArgumentException.And.Message.EqualTo(message);
        }

        internal static IResolveConstraint CanNotRegisterAbstractNavigationManagerType(Type navigationManagerType)
        {
            var message = ExceptionMessages.CanNotRegisterAbstractNavigationManagerType(navigationManagerType);
            return Throws.ArgumentException.And.Message.EqualTo(message);
        }

        internal static IResolveConstraint FrameControlIsNotOfFrameControlType(object frameControl, Type frameControlType)
        {
            var message = ExceptionMessages.FrameControlIsNotOfFrameControlType(frameControl, frameControlType);
            return Throws.ArgumentException.And.Message.EqualTo(message);
        }

        internal static IResolveConstraint NavigationManagerTypeIsNotFound()
        {
            var message = ExceptionMessages.NavigationManagerTypeIsNotFound();
            return Throws.InvalidOperationException.And.Message.EqualTo(message);
        }

        internal static IResolveConstraint FrameControlTypeIsNotFound()
        {
            var message = ExceptionMessages.FrameControlTypeIsNotFound();
            return Throws.InvalidOperationException.And.Message.EqualTo(message);
        }
    }
}
