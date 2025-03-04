﻿using System.Diagnostics.CodeAnalysis;
using Egor92.MvvmNavigation.Core.ContractTests.Internal;
using Egor92.MvvmNavigation.Tests.Common;
using NUnit.Framework;

namespace Egor92.MvvmNavigation.Core.UnitTests.InternalTests
{
    [TestFixture]
    internal class RegistrationDataTests
    {
        [Test]
        [SuppressMessage("ReSharper", "ConvertToLocalFunction")]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void ViewModelFuncIsNull_ThrowException()
        {
            //Act
            TestDelegate action = () =>
            {
                new RegistrationData(null, () => new object());
            };

            //Assert
            Assert.That(action, ThrowsException.NullArgument(ArgumentNames.ViewModelFunc));
        }

        [Test]
        [SuppressMessage("ReSharper", "ConvertToLocalFunction")]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void ViewFuncIsNull_ThrowException()
        {
            //Act
            TestDelegate action = () =>
            {
                new RegistrationData(() => new object(), null);
            };

            //Assert
            Assert.That(action, ThrowsException.NullArgument(ArgumentNames.ViewFunc));
        }
    }
}
