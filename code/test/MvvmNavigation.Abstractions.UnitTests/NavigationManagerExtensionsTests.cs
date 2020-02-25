using Moq;
using NUnit.Framework;

namespace Egor92.MvvmNavigation.Abstractions.UnitTests
{
    [TestFixture]
    public class NavigationManagerExtensionsTests
    {
        private readonly Mock<INavigationManager> _navigationManager = new Mock<INavigationManager>();

        [Test]
        public void Navigate_WithoutParameter_ArgIsNull()
        {
            //Arrange
            string key = TestContext.CurrentContext.Random.GetString();

            //Act
            _navigationManager.Object.Navigate(key);

            //Assert
            _navigationManager.Verify(x => x.Navigate(key, null), Times.Once);
        }
    }
}
