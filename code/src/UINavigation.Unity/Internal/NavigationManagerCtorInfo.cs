using System;
using Unity.Injection;

namespace Egor92.UINavigation.Unity.Internal
{
    internal static class NavigationManagerCtorInfo
    {
        internal static InjectionConstructor GetInjectionConstructor(Type frameControlType)
        {
            object[] resolvedParameters =
            {
                new ResolvedParameter(frameControlType, Args.FrameControl.Name),
                new ResolvedParameter<IDataStorage>()
            };
            return new InjectionConstructor(resolvedParameters);
        }

        internal static class Args
        {
            internal static class FrameControl
            {
                internal const string Name = "frameControl";
            }
        }
    }
}
