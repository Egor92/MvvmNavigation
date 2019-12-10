using System;
using System.Linq;

namespace Egor92.UINavigation.Internal
{
    internal class NavigationManagerTypeProvider : INavigationManagerTypeProvider
    {
        private static readonly string[] AssemblyNames =
        {
            "UINavigation.Wpf"
        };

        public Type GetNavigationManagerType()
        {
            return AssemblyNames.Select(assemblyName => $"Egor92.{assemblyName}.NavigationManager, {assemblyName}")
                                .Select(Type.GetType)
                                .FirstOrDefault(x => x != null);
        }
    }
}
