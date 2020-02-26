using System;

namespace Egor92.MvvmNavigation.Internal
{
    internal interface IFrameControlTypeProvider
    {
        Type GetFrameControlType(Type navigationManagerType);
    }
}
