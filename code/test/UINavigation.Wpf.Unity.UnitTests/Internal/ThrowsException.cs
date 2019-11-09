using Egor92.UINavigation.Wpf.Unity.Internal;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Egor92.UINavigation.Wpf.Unity.UnitTests.Internal
{
    public static class ThrowsException
    {
        public static IResolveConstraint CanNotRegisterInterfaceAsViewModel<TViewModel>()
        {
            var message = ExceptionMessages.CanNotRegisterInterfaceAsViewModel<TViewModel>();
            return Throws.ArgumentException.And.Message.EqualTo(message);
        }

        public static IResolveConstraint CanNotRegisterAbstractClassAsViewModel<TViewModel>()
        {
            var message = ExceptionMessages.CanNotRegisterAbstractClassAsViewModel<TViewModel>();
            return Throws.ArgumentException.And.Message.EqualTo(message);
        }

        public static IResolveConstraint CanNotRegisterAbstractClassAsView<TView>()
        {
            var message = ExceptionMessages.CanNotRegisterAbstractClassAsView<TView>();
            return Throws.ArgumentException.And.Message.EqualTo(message);
        }
    }
}
