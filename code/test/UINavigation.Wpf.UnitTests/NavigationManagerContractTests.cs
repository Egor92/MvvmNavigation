using System.Threading;
using System.Windows.Controls;
using Egor92.UINavigation.Abstractions.ContractTests;
using NUnit.Framework;

namespace Egor92.UINavigation.Wpf.UnitTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    [Apartment(ApartmentState.STA)]
    public class NavigationManagerContractTests : NavigationManagerContractTestsBase<NavigationManager>
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        protected override NavigationManager CreateNavigationManager()
        {
            var frameControl = new ContentControl();
            return new NavigationManager(frameControl);
        }

        protected override void SetupCanNavigate(NavigationManager navigationManager, string navigationKey, bool canNavigate)
        {
            if (canNavigate)
            {
                navigationManager.Register(navigationKey, () => null, () => null);
            }
        }
    }
}
