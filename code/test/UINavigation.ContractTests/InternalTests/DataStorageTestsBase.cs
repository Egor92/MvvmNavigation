using System.Diagnostics.CodeAnalysis;
using Egor92.UINavigation.ContractTests.Internal;
using NUnit.Framework;
using ThrowsException = Egor92.UINavigation.Tests.Common.ThrowsException;

namespace Egor92.UINavigation.ContractTests.InternalTests
{
    public abstract class DataStorageTestsBase<TDataStorage>
        where TDataStorage : IDataStorage

    {
        private TDataStorage _dataStorage;
        private NavigationData _navigationData;

        [SetUp]
        public virtual void SetUp()
        {
            _dataStorage = CreateDataStorage();
            _navigationData = new NavigationData(() => new object(), () => new object());
        }

        protected abstract TDataStorage CreateDataStorage();

        [Test]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        [SuppressMessage("ReSharper", "ConvertToLocalFunction")]
        public void Add_KeyIsNull_ThrowException()
        {
            //Act
            TestDelegate action = () =>
            {
                _dataStorage.Add(null, _navigationData);
            };

            //Assert
            Assert.That(action, ThrowsException.NullArgument(ArgumentNames.Key));
        }

        [Test]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        [SuppressMessage("ReSharper", "ConvertToLocalFunction")]
        public void Add_NavigationDataIsNull_ThrowException()
        {
            //Arrange
            const string key = "key";

            //Act
            TestDelegate action = () =>
            {
                _dataStorage.Add(key, null);
            };

            //Assert
            Assert.That(action, ThrowsException.NullArgument(ArgumentNames.NavigationData));
        }

        [Test]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        [SuppressMessage("ReSharper", "ConvertToLocalFunction")]
        public void IsExist_KeyIsNull_ThrowException()
        {
            //Act
            TestDelegate action = () =>
            {
                _dataStorage.IsExist(null);
            };

            //Assert
            Assert.That(action, ThrowsException.NullArgument(ArgumentNames.Key));
        }

        [Test]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        [SuppressMessage("ReSharper", "ConvertToLocalFunction")]
        public void Get_KeyIsNull_ThrowException()
        {
            //Act
            TestDelegate action = () =>
            {
                _dataStorage.Get(null);
            };

            //Assert
            Assert.That(action, ThrowsException.NullArgument(ArgumentNames.Key));
        }

        [Test]
        public void IsExist_KeyWasAdded_ReturnTrue()
        {
            //Arrange
            const string key = "key";

            //Act
            _dataStorage.Add(key, _navigationData);

            bool isExist = _dataStorage.IsExist(key);

            //Assert
            Assert.That(isExist, Is.True);
        }

        [Test]
        public void Get_IsExist_NavigationDataIsNotNull()
        {
            //Arrange
            const string key = "key";
            SetupIsExistTrue(_dataStorage, key);

            //Act
            var navigationData = _dataStorage.Get(key);

            //Assert
            Assert.That(navigationData, Is.Not.Null);
        }

        protected abstract void SetupIsExistTrue(TDataStorage dataStorage, string key);
    }
}
