using System.Diagnostics.CodeAnalysis;
using Egor92.UINavigation.Wpf.ContractTests.InternalTests;
using Egor92.UINavigation.Wpf.Unity.UnitTests.Internal;
using NUnit.Framework;
using Unity;
using ThrowsException = Egor92.UINavigation.Tests.Common.ThrowsException;

namespace Egor92.UINavigation.Wpf.Unity.UnitTests
{
    [TestFixture]
    [SuppressMessage("ReSharper", "ConvertToLocalFunction")]
    public class UnityDataStorageTests : DataStorageTestsBase<UnityDataStorage>
    {
        private IUnityContainer _unityContainer;

        [SetUp]
        public override void SetUp()
        {
            _unityContainer = new UnityContainer();
            base.SetUp();
        }

        [TearDown]
        public void TearDown()
        {
            _unityContainer.Dispose();
        }

        protected override UnityDataStorage CreateDataStorage()
        {
            return new UnityDataStorage(_unityContainer);
        }

        protected override void SetupIsExistTrue(UnityDataStorage dataStorage, string key)
        {
            var navigationData = new NavigationData(() => new object(), () => new object());
            _unityContainer.RegisterInstance<NavigationData>(key, navigationData);
        }

        [Test]
        public void UnityContainerIsNull_ThrowException()
        {
            //Act
            TestDelegate action = () =>
            {
                new UnityDataStorage(null);
            };

            //Assert
            Assert.That(action, ThrowsException.NullArgument(ArgumentNames.UnityContainer));
        }
    }
}
