using System;
using Egor92.MvvmNavigation.Unity.Internal;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Egor92.MvvmNavigation.Wpf.Unity.IntegrationTests.Internal
{
    internal static class ThrowsException
    {
        internal static IResolveConstraint FrameControlIsNotOfFrameControlType(object frameControl, Type frameControlType)
        {
            var message = ExceptionMessages.FrameControlIsNotOfFrameControlType(frameControl, frameControlType);
            return Throws.ArgumentException.And.Message.EqualTo(message);
        }
    }
}
