using System;
using Autofac.Extras.Moq;
using ClassLibrary1;
using CommonServiceLocator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace TestProject1
{
    public class UnitTest1
    {
        [Fact]
        public void UsingAutoWiredDependencyInConstructorTest()
        {
            Startup.Configure();

            MockOutput output = (MockOutput)ServiceLocator.Current.GetService<IOutput>();
            ServiceLocator.Current.GetService<IDateWriter>().WriteDate();

            Assert.True(output.Called);
            Assert.Equal(DateTime.Now.ToShortDateString(), output.With);
        }

        [Fact]
        public void UsingManuallyCreatedAndRegisteredDependencyInConstructorTest()
        {
            Startup.Configure();

            MockLogger logger = (MockLogger) ServiceLocator.Current.GetService<ILogger>();
            AsyncMessageWriter writer = new AsyncMessageWriter(ServiceLocator.Current.GetService<ILogger<AsyncMessageWriter>>());
            writer.WriteMessage("Test string");
            Assert.True(logger.Called);
            Assert.Equal("WriteMessage called. Message: Test string", logger.With);
        }

        [Fact]
        public void UsingMoqToInstantiateWithMockedObjectTest()
        {
            using AutoMock mock = AutoMock.GetLoose();
            
            mock.Mock<IOutput>().Setup(output => output.Write(It.IsAny<string>())).Verifiable();
            TodayWriter writer = mock.Create<TodayWriter>();
            writer.WriteDate();

            mock.Mock<IOutput>().Verify();
        }
    }
}