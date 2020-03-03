using System.Diagnostics.CodeAnalysis;
using Egor92.MvvmNavigation.Core.ContractTests.Internal;
using NUnit.Framework;
using ThrowsException = Egor92.MvvmNavigation.Tests.Common.ThrowsException;

namespace Egor92.MvvmNavigation.Core.ContractTests
{
    public abstract class ViewInteractionStrategyContractTests<TViewInteractionStrategy, TContentControl, TControlWithDataContext>
        where TViewInteractionStrategy : IViewInteractionStrategy
        where TContentControl : new()
        where TControlWithDataContext : new()
    {
        private TViewInteractionStrategy _viewInteractionStrategy;

        public virtual void SetUp()
        {
            _viewInteractionStrategy = CreateViewInteractionStrategy();
        }

        protected abstract TViewInteractionStrategy CreateViewInteractionStrategy();

        protected abstract object GetContent(TContentControl contentControl);

        [Test]
        public void GetContent_ControlIsNull_ThrowException()
        {
            //Act
            void Action()
            {
                _viewInteractionStrategy.GetContent(null);
            }

            //Assert
            Assert.That(Action, ThrowsException.NullArgument(ArgumentNames.Control));
        }

        [Test]
        public void GetContent_ContentWasSet_ReturnTheContent()
        {
            //Arrange
            var control = new TContentControl();
            var originalContent = new object();
            _viewInteractionStrategy.SetContent(control, originalContent);

            //Act
            var content = _viewInteractionStrategy.GetContent(control);

            //Assert
            Assert.That(content, Is.EqualTo(originalContent));
        }

        [Test]
        public void GetContent_ContentHasNoContent_ReturnNull()
        {
            //Arrange
            var control = new object();

            //Act
            var content = _viewInteractionStrategy.GetContent(control);

            //Assert
            Assert.That(content, Is.Null);
        }

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
                _viewInteractionStrategy.SetContent(null, new object());
            }

            //Assert
            Assert.That(Action, ThrowsException.NullArgument(ArgumentNames.Control));
        }

        [Test]
        public void SetContent_ContentIsNotNull_SetContentToControl()
        {
            //Arrange
            var contentControl = new TContentControl();
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
            var contentControl = new TContentControl();

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
            var controlWithDataContext = new TControlWithDataContext();

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
            var contentControl = new TContentControl();

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
            static void DoSomething()
            {
            }

            //Act
            void Action()
            {
                _viewInteractionStrategy.InvokeInUIThread(null, DoSomething);
            }

            //Assert
            Assert.That(Action, ThrowsException.NullArgument(ArgumentNames.Control));
        }

        [Test]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void InvokeInDispatcher_ActionIsNull_ThrowException()
        {
            //Arrange
            var contentControl = new TContentControl();

            //Act
            void Action()
            {
                _viewInteractionStrategy.InvokeInUIThread(contentControl, null);
            }

            //Assert
            Assert.That(Action, ThrowsException.NullArgument(ArgumentNames.Action));
        }

        [Test]
        public void InvokeInDispatcher_DispatcherThread_ActionIsInvoked()
        {
            //Arrange
            var contentControl = new TContentControl();
            bool isActionInvoked = false;

            //Act
            _viewInteractionStrategy.InvokeInUIThread(contentControl, () =>
            {
                isActionInvoked = true;
            });

            //Assert
            Assert.That(isActionInvoked, Is.True);
        }
    }
}
