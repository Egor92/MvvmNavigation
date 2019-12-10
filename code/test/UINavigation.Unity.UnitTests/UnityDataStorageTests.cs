﻿using System.Diagnostics.CodeAnalysis;
using Egor92.UINavigation.ContractTests.InternalTests;
using Egor92.UINavigation.Unity.UnitTests.Internal;
using NUnit.Framework;
using Unity;
using ThrowsException = Egor92.UINavigation.Tests.Common.ThrowsException;

namespace Egor92.UINavigation.Unity.UnitTests
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
            _unityContainer.RegisterInstance(key, navigationData);
        }

        [Test]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
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
