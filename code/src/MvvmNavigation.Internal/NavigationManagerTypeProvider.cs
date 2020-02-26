using System;
using System.Linq;

namespace Egor92.MvvmNavigation.Internal
{
    internal class NavigationManagerTypeProvider : INavigationManagerTypeProvider
    {
        private static readonly string[] AssemblyNames =
        {
            "MvvmNavigation.Wpf"
        };

        public Type GetNavigationManagerType()
        {
            return AssemblyNames.Select(assemblyName => $"Egor92.MvvmNavigation.NavigationManager, {assemblyName}")
                                .Select(Type.GetType)
                                .FirstOrDefault(x => x != null);
        }
    }
}
