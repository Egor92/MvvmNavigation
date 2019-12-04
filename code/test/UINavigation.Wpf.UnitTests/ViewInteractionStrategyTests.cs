using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Egor92.UINavigation.ContractTests;
using NUnit.Framework;

namespace Egor92.UINavigation.Wpf.UnitTests
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class ViewInteractionStrategyTests : ViewInteractionStrategyContractTests<ViewInteractionStrategy, ContentControl, FrameworkElement>
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

        protected override ContentControl CreateContentControl()
        {
            return new ContentControl();
        }

        protected override FrameworkElement CreateControlWithDataContext()
        {
            return new FrameworkElement();
        }

        protected override object GetContent(ContentControl contentControl)
        {
            return contentControl.Content;
        }
    }
}
