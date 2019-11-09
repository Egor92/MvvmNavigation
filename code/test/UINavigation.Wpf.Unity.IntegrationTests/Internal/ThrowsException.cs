using System;
using Egor92.UINavigation.Unity.Internal;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Egor92.UINavigation.Wpf.Unity.IntegrationTests.Internal
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
