using Egor92.UINavigation.Wpf.Internal;
using NUnit.Framework;

namespace Egor92.UINavigation.Wpf.Tests.InternalTests
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
