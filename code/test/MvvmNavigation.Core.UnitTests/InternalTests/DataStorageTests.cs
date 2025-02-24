using Egor92.MvvmNavigation.Core.ContractTests.InternalTests;
using Egor92.MvvmNavigation.Internal;
using NUnit.Framework;

namespace Egor92.MvvmNavigation.Core.UnitTests.InternalTests
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
            var navigationData = new RegistrationData(() => new object(), () => new object());
            dataStorage.Add(key, navigationData);
        }
    }
}
