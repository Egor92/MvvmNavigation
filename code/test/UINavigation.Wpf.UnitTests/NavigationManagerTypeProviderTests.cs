using System;
using Egor92.UINavigation.Internal.ContractTests;
using NUnit.Framework;

namespace Egor92.UINavigation.Wpf.UnitTests
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
