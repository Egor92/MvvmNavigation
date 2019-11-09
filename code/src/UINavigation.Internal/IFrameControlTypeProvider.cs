using System;

namespace Egor92.UINavigation.Internal
{
    internal interface IFrameControlTypeProvider
    {
        Type GetFrameControlType(Type navigationManagerType);
    }
}
