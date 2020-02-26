using Egor92.MvvmNavigation.Internal.UnitTests.Internal;
using NUnit.Framework;

namespace Egor92.MvvmNavigation.Internal.UnitTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    [RunInApplicationDomain]
    public class FrameControlTypeProviderTests
    {
        private FrameControlTypeProvider _frameControlTypeProvider;

        [SetUp]
        public void SetUp()
        {
            _frameControlTypeProvider = new FrameControlTypeProvider();
        }

        [Test]
        public void GetFrameControlType_TypeHasNoNavigationManagerAttribute_ReturnNull()
        {
            //Arrange
            var generatedType = ClassBuilder.CreateNewClass()
                                            .Build();

            //Act
            var frameControlType = _frameControlTypeProvider.GetFrameControlType(generatedType);

            //Assert
            Assert.That(frameControlType, Is.Null);
        }

        [Test]
        public void GetFrameControlType_InputTypeHasNavigationManagerAttribute_ReturnFrameControlType()
        {
            //Arrange
            var originalFrameControlType = typeof(object);
            var generatedType = ClassBuilder.CreateNewClass()
                                            .WithNavigationManagerAttribute(originalFrameControlType)
                                            .Build();

            //Act
            var frameControlType = _frameControlTypeProvider.GetFrameControlType(generatedType);

            //Assert
            Assert.That(frameControlType, Is.EqualTo(originalFrameControlType));
        }

        [Test]
        public void GetFrameControlType_FrameControlTypeIsNull_ReturnPlatformSpecificTypeInfoWithNullAsFrameControlType()
        {
            //Arrange
            var generatedType = ClassBuilder.CreateNewClass()
                                            .WithNavigationManagerAttribute(null)
                                            .Build();

            //Act
            var frameControlType = _frameControlTypeProvider.GetFrameControlType(generatedType);

            //Assert
            Assert.That(frameControlType, Is.Null);
        }
    }
}
