using System;

namespace Egor92.UINavigation.Internal
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
