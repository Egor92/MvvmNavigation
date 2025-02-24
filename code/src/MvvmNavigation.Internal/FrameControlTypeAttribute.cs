using System;

namespace Egor92.MvvmNavigation.Internal
{
    internal class FrameControlTypeAttribute : Attribute
    {
        internal FrameControlTypeAttribute(Type frameControlType)
        {
            FrameControlType = frameControlType;
        }

        internal Type FrameControlType { get; }
    }
}
