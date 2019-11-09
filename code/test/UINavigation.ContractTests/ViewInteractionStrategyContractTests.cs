using System.Diagnostics.CodeAnalysis;
using Egor92.UINavigation.ContractTests.Internal;
using Egor92.UINavigation.Tests.Common;
using NUnit.Framework;

namespace Egor92.UINavigation.ContractTests
{
    public abstract class ViewInteractionStrategyContractTests<TViewInteractionStrategy, TContentControl, TControlWithDataContext>
        where TViewInteractionStrategy : IViewInteractionStrategy
    {
        private TViewInteractionStrategy _viewInteractionStrategy;

        public virtual void SetUp()
        {
            _viewInteractionStrategy = CreateViewInteractionStrategy();
        }

        protected abstract TViewInteractionStrategy CreateViewInteractionStrategy();

        protected abstract TContentControl CreateContentControl();

        protected abstract TControlWithDataContext CreateControlWithDataContext();

        protected abstract object GetContent(TContentControl contentControl);

        [Test]
        public void SetContent_ControlHasNotContentProperty_DoNotThrowException()
        {
            //Arrange
            var controlWithoutContent = new object();

            //Act
            void Action()
            {
                _viewInteractionStrategy.SetContent(controlWithoutContent, new object());
            }

            //Assert
            Assert.That(Action, Throws.Nothing);
        }

        [Test]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void SetContent_ControlIsNull_ThrowException()
        {
            //Act
            void Action()
            {
                _viewInteractionStrategy.SetDataContext(null, new object());
            }

            //Assert
            Assert.That(Action, ThrowsException.NullArgument(ArgumentNames.Control));
        }

        [Test]
        public void SetContent_ContentIsNotNull_SetContentToControl()
        {
            //Arrange
            var contentControl = CreateContentControl();
            var content = new object();

            //Act
            _viewInteractionStrategy.SetContent(contentControl, content);

            //Assert
            Assert.That(GetContent(contentControl), Is.EqualTo(content));
        }

        [Test]
        public void SetContent_ContentIsNull_ControlHasNullAsContent()
        {
            //Arrange
            var contentControl = CreateContentControl();

            //Act
            _viewInteractionStrategy.SetContent(contentControl, null);

            //Assert
            Assert.That(GetContent(contentControl), Is.Null);
        }

        [Test]
        public void GetDataContext_ControlHasNotDataContext_ReturnNull()
        {
            //Arrange
            var controlWithoutDataContext = new object();

            //Act
            var dataContext = _viewInteractionStrategy.GetDataContext(controlWithoutDataContext);

            //Assert
            Assert.That(dataContext, Is.Null);
        }

        [Test]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void GetDataContext_ControlIsNull_ThrowException()
        {
            //Act
            void Action()
            {
                _viewInteractionStrategy.GetDataContext(null);
            }

            //Assert
            Assert.That(Action, ThrowsException.NullArgument(ArgumentNames.Control));
        }

        [Test]
        public void GetDataContext_DataContextHadBeenSet_ReturnThisDataContext()
        {
            //Arrange
            var originalDataContext = new object();
            var controlWithDataContext = CreateControlWithDataContext();

            _viewInteractionStrategy.SetDataContext(controlWithDataContext, originalDataContext);

            //Act
            var gottenDataContext = _viewInteractionStrategy.GetDataContext(controlWithDataContext);

            //Assert
            Assert.That(gottenDataContext, Is.EqualTo(originalDataContext));
        }

        [Test]
        public void SetDataContext_ControlHasNotDataContext_DoNotThrowException()
        {
            //Arrange
            var controlWithoutDataContext = new object();

            //Act
            void Action()
            {
                _viewInteractionStrategy.SetDataContext(controlWithoutDataContext, new object());
            }

            //Assert
            Assert.That(Action, Throws.Nothing);
        }

        [Test]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void SetDataContext_ControlIsNull_ThrowException()
        {
            //Act
            void Action()
            {
                _viewInteractionStrategy.SetDataContext(null, new object());
            }

            //Assert
            Assert.That(Action, ThrowsException.NullArgument(ArgumentNames.Control));
        }

        [Test]
        public void SetDataContext_DataContextIsNull_DoNotThrowException()
        {
            //Arrange
            var contentControl = CreateContentControl();

            //Act
            void Action()
            {
                _viewInteractionStrategy.SetDataContext(contentControl, null);
            }

            //Assert
            Assert.That(Action, Throws.Nothing);
        }

        [Test]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void InvokeInDispatcher_ControlIsNull_ThrowException()
        {
            //Arrange
            void DoSomething()
            {
            }

            //Act
            void Action()
            {
                _viewInteractionStrategy.InvokeInDispatcher(null, DoSomething);
            }

            //Assert
            Assert.That(Action, ThrowsException.NullArgument(ArgumentNames.Control));
        }

        [Test]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void InvokeInDispatcher_ActionIsNull_ThrowException()
        {
            //Arrange
            var contentControl = CreateContentControl();

            //Act
            void Action()
            {
                _viewInteractionStrategy.InvokeInDispatcher(contentControl, null);
            }

            //Assert
            Assert.That(Action, ThrowsException.NullArgument(ArgumentNames.Action));
        }

        [Test]
        public void InvokeInDispatcher_DispatcherThread_ActionIsInvoked()
        {
            //Arrange
            var contentControl = CreateContentControl();
            bool isActionInvoked = false;

            //Act
            _viewInteractionStrategy.InvokeInDispatcher(contentControl, () =>
            {
                isActionInvoked = true;
            });

            //Assert
            Assert.That(isActionInvoked, Is.True);
        }
    }
}
