using System.Diagnostics.CodeAnalysis;
using Egor92.MvvmNavigation.Tests.Common;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Egor92.MvvmNavigation.Abstractions.ContractTests
{
    [SuppressMessage("ReSharper", "ConvertToLocalFunction")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public abstract class NavigationManagerContractTests<TNavigationManager>
        where TNavigationManager : INavigationManager
    {
        #region Fields

        private string _navigationKey;
        protected TNavigationManager _navigationManager;

        #endregion

        #region Setup

        public virtual void SetUp()
        {
            _navigationKey = "navigationKey";
            _navigationManager = CreateNavigationManager();
        }

        protected abstract TNavigationManager CreateNavigationManager();

        #endregion

        #region CanNavigate_KeyIsNull_ThrowException

        [Test]
        public void CanNavigate_KeyIsNull_ThrowException()
        {
            //Act
            TestDelegate action = () =>
            {
                _navigationManager.CanNavigate(null);
            };

            //Assert
            Assert.That(action, Throws.ArgumentNullException);
        }

        #endregion

        #region Navigate_ThrowExceptionDependingOnCanNavigate

        public static TestCaseData[] Navigate_ThrowExceptionDependingOnCanNavigate_CaseSource()
        {
            return new[]
            {
                TestCaseDataBuilder.Create()
                                   .WithParameter(false, "if can't navigate")
                                   .WithParameter(Throws.InvalidOperationException, "throw InvalidOperationException")
                                   .Build(),
                TestCaseDataBuilder.Create()
                                   .WithParameter(true, "if can navigate")
                                   .WithParameter(Throws.Nothing, "throw nothing")
                                   .Build(),
            };
        }

        [TestCaseSource(nameof(Navigate_ThrowExceptionDependingOnCanNavigate_CaseSource))]
        public void Navigate_ThrowExceptionDependingOnCanNavigate(bool canNavigate, Constraint throwExceptionConstraint)
        {
            //Arrange
            SetupCanNavigate(_navigationManager, _navigationKey, canNavigate);

            if (_navigationManager.CanNavigate(_navigationKey) != canNavigate)
            {
                Assert.Fail($"The test is configured incorrectly. SetupCanNavigate method didn't set CanNavigate(navigationKey) to {canNavigate}");
            }

            //Act
            TestDelegate action = () =>
            {
                _navigationManager.Navigate(_navigationKey, null);
            };

            //Assert
            Assert.That(action, throwExceptionConstraint);
        }

        protected abstract void SetupCanNavigate(TNavigationManager navigationManager, string navigationKey, bool canNavigate);

        #endregion
    }
}
