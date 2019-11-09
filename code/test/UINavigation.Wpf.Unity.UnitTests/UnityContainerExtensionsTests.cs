using System.Threading;
using NUnit.Framework;
using Unity;

namespace Egor92.UINavigation.Wpf.Unity.UnitTests
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public partial class UnityContainerExtensionsTests
    {
        private UnityContainer _unityContainer;
        private string _navigationKey;

        [SetUp]
        public void SetUp()
        {
            _unityContainer = new UnityContainer();
            _navigationKey = TestContext.CurrentContext.Random.GetString();
        }

        [TearDown]
        public void TearDown()
        {
            _unityContainer?.Dispose();
        }
    }
}
