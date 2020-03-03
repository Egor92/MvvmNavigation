using System.Threading;
using Avalonia.Controls;
using Egor92.MvvmNavigation.Core.ContractTests;
using NUnit.Framework;

namespace Egor92.MvvmNavigation.Avalonia.UnitTests
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class ViewInteractionStrategyTests : ViewInteractionStrategyContractTests<ViewInteractionStrategy, ContentControl, Control>
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        protected override ViewInteractionStrategy CreateViewInteractionStrategy()
        {
            return new ViewInteractionStrategy();
        }

        protected override object GetContent(ContentControl contentControl)
        {
            return contentControl.Content;
        }
    }
}
