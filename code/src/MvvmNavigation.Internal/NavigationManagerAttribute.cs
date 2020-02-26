using System;

namespace Egor92.MvvmNavigation.Internal
{
    internal class NavigationManagerAttribute : Attribute
    {
        internal NavigationManagerAttribute(Type frameControlType)
        {
            FrameControlType = frameControlType;
        }

        internal Type FrameControlType { get; }
    }
}
