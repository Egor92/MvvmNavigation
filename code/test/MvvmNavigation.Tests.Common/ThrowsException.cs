using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Egor92.MvvmNavigation.Tests.Common
{
    public static class ThrowsException
    {
        public static IResolveConstraint NullArgument(string argumentName)
        {
            return Throws.ArgumentNullException.With.Property("ParamName").EqualTo(argumentName);
        }
    }
}
