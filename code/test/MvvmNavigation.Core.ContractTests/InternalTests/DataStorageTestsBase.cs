using System.Diagnostics.CodeAnalysis;
using Egor92.MvvmNavigation.Core.ContractTests.Internal;
using NUnit.Framework;
using ThrowsException = Egor92.MvvmNavigation.Tests.Common.ThrowsException;

namespace Egor92.MvvmNavigation.Core.ContractTests.InternalTests
{
    public abstract class DataStorageTestsBase<TDataStorage>
        where TDataStorage : IDataStorage

    {
        private TDataStorage _dataStorage;
        private RegistrationData _registrationData;

        [SetUp]
        public virtual void SetUp()
        {
            _dataStorage = CreateDataStorage();
            _registrationData = new RegistrationData(() => new object(), () => new object());
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
                _dataStorage.Add(null, _registrationData);
            };

            //Assert
            Assert.That(action, ThrowsException.NullArgument(ArgumentNames.Key));
        }

        [Test]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        [SuppressMessage("ReSharper", "ConvertToLocalFunction")]
        public void Add_RegistrationDataIsNull_ThrowException()
        {
            //Arrange
            const string key = "key";

            //Act
            TestDelegate action = () =>
            {
                _dataStorage.Add(key, null);
            };

            //Assert
            Assert.That(action, ThrowsException.NullArgument(ArgumentNames.RegistrationData));
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
            _dataStorage.Add(key, _registrationData);

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
