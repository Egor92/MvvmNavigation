using System;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Egor92.UINavigation.Tests.Common
{
    public static class ThrowsException
    {
        public static class InnerException
        {
            public static IResolveConstraint NullArgument(string argumentName)
            {
                var message = ExceptionMessages.NullArgument(argumentName);
                return Throws.InnerException.TypeOf<ArgumentNullException>()
                             .And.InnerException.Message.EqualTo(message);
            }
        }

        public static IResolveConstraint NullArgument(string argumentName)
        {
            var message = ExceptionMessages.NullArgument(argumentName);
            return Throws.ArgumentNullException.And.Message.EqualTo(message);
        }
    }
}
