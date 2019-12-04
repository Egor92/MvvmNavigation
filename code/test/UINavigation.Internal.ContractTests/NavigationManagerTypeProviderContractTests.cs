using System;
using NUnit.Framework;

namespace Egor92.UINavigation.Internal.ContractTests
{
    public abstract class NavigationManagerTypeProviderContractTests
    {
        private NavigationManagerTypeProvider _navigationManagerTypeProvider;

        [SetUp]
        public void SetUp()
        {
            _navigationManagerTypeProvider = new NavigationManagerTypeProvider();
        }

        [Test]
        public void GetNavigationManagerType_NavigationManagerTypeIsFound()
        {
            //Act
            var navigationManagerType = _navigationManagerTypeProvider.GetNavigationManagerType();

            //Assert
            var requiredNavigationManagerType = GetRequiredNavigationManagerType();
            Assert.That(navigationManagerType, Is.EqualTo(requiredNavigationManagerType));
        }

        protected abstract Type GetRequiredNavigationManagerType();
    }
}
