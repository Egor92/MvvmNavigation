using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Egor92.MvvmNavigation.Core.ContractTests.Internal
{
    internal static class ThrowsException
    {
        internal static IResolveConstraint NullArgument(string argumentName)
        {
            return Throws.ArgumentNullException.With.Property("ParamName").EqualTo(argumentName);
        }

        internal static IResolveConstraint CanNotRegisterKeyTwice()
        {
            var message = MvvmNavigation.Internal.ExceptionMessages.CanNotRegisterKeyTwice;
            return Throws.InvalidOperationException.With.Message.EqualTo(message);
        }

        internal static IResolveConstraint KeyIsNotRegistered(string navigationKey)
        {
            var message = MvvmNavigation.Internal.ExceptionMessages.KeyIsNotRegistered(navigationKey);
            return Throws.InvalidOperationException.With.Message.EqualTo(message);
        }
    }
}
