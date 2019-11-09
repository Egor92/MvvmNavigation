using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Egor92.UINavigation.Tests.Common
{
    public static class ThrowsException
    {
        public static IResolveConstraint NullArgument(string argumentName)
        {
            var message = ExceptionMessages.NullArgument(argumentName);
            return Throws.ArgumentNullException.And.Message.EqualTo(message);
        }
    }
}
