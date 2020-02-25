using System;
using Egor92.MvvmNavigation.Internal.ContractTests;
using NUnit.Framework;

namespace Egor92.MvvmNavigation.Wpf.UnitTests
{
    [TestFixture]
    public class NavigationManagerTypeProviderTests : NavigationManagerTypeProviderContractTests
    {
        protected override Type GetRequiredNavigationManagerType()
        {
            return typeof(NavigationManager);
        }
    }
}
