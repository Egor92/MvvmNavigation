using System.Windows.Controls;
using Unity.Injection;

namespace Egor92.UINavigation.Wpf.Unity.Internal
{
    internal static class NavigationManagerCtorInfo
    {
        private static readonly object[] ResolvedParameters =
        {
            new ResolvedParameter<ContentControl>(Args.FrameControl.Name),
            new ResolvedParameter<IDataStorage>()
        };

        internal static readonly InjectionConstructor InjectionConstructor = new InjectionConstructor(ResolvedParameters);

        internal static class Args
        {
            internal static class FrameControl
            {
                internal const string Name = "frameControl";
            }
        }
    }
}
