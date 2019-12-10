using Egor92.UINavigation.ContractTests.InternalTests;
using Egor92.UINavigation.Internal;
using NUnit.Framework;

namespace Egor92.UINavigation.UnitTests.InternalTests
{
    [TestFixture]
    internal class DataStorageTests : DataStorageTestsBase<DataStorage>
    {
        protected override DataStorage CreateDataStorage()
        {
            return new DataStorage();
        }

        protected override void SetupIsExistTrue(DataStorage dataStorage, string key)
        {
            var navigationData = new NavigationData(() => new object(), () => new object());
            dataStorage.Add(key, navigationData);
        }
    }
}
