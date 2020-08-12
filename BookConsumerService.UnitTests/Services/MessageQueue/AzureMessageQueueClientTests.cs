using AutoFixture;
using BookConsumerService.Services.Interfaces;
using BookConsumerService.Services.MessageQueue;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BookConsumerService.UnitTests.Services
{
    public class AzureMessageQueueClientTests
    {
        private readonly MockRepository mockRepository;
        private readonly Fixture fixture;
        public AzureMessageQueueClientTests()
        {
            fixture = new Fixture();
            mockRepository = new MockRepository(MockBehavior.Strict);
        }
        //   rem code because the corporate network forbids connecting to the Azure Message Queue
        //[Fact]
        //private async Task ReceiveMessageAsync_StateUnderTest_ExpectedBehavior()
        //{
            ////Arrage
            //var logger = new Mock<ILogger>();
            //var queueClientMock = mockRepository.Create<IQueueClient>();
            //var saveData = mockRepository.Create<ISaveData>();
            //queueClientMock.Setup(x =>
            //            x.SendAsync(It.IsAny<Message>())).Returns(Task.CompletedTask);//.Verifiable();
            //queueClientMock.Setup(x => x.CloseAsync()).Returns(Task.CompletedTask);
            //string connectString = "Endpoint=sb://anhnguyen.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=qTKznJRadfRhedcnYAR6NC9CVM9DPEoMbcF9PSIy27Q=";
            //string queueName = "myQueue";
            //var service = new AzureMessageQueueClient(saveData.Object, connectString, queueName, logger.Object);
            ////Act
            //var result = await service.ReceiveMessageAsync();
            ////Assert
            //Assert.True(result);
            //mockRepository.VerifyAll();
     //   }

    }
}
