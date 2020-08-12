using AutoFixture;
using BookConsumerService.Models.DomainModels;
using BookConsumerService.Models.DTOs;
using BookConsumerService.Services;
using BookConsumerService.Services.Interfaces;
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
    public class SaveDataTests
    {
        private readonly MockRepository _mockRepository;
        private readonly Fixture _fixture;
        public SaveDataTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _fixture = new Fixture();
        }
        [Fact]
        public void ExecuteQueryDELETE_StateUnderTest_ExpectedBehavior()
        {
            //Arrange
            Mock<ILogger> logger = new Mock<ILogger>();
            Mock<IBookService> bookService = _mockRepository.Create<IBookService>();
            var bookdto = _fixture.Create<BookDto>();
            MessageInfo messageInfo = _fixture.Build<MessageInfo>().With(x => x.BookDto, bookdto).With(x => x.ActionType, "DELETE").Create();
            bookService.Setup(x => x.DeleteBook(bookdto.Id)).ReturnsAsync(true);
            string message = JsonConvert.SerializeObject(messageInfo);
            var saveData = new SaveData(bookService.Object, logger.Object);
            //Act
            var result = saveData.ExecuteQuery(message);
            //Assert
            Assert.True(result.Result);
            _mockRepository.VerifyAll();
        }
        [Theory]
        [InlineData("POST")]
        [InlineData("PUT")]
        public void ExecuteQuery_StateUnderTest_ExpectedBehavior(string actionType)
        {
            //Arrange
            Mock<ILogger> logger = new Mock<ILogger>();
            Mock<IBookService> bookService = _mockRepository.Create<IBookService>();
            var bookdto = _fixture.Create<BookDto>();
            MessageInfo messageInfo = _fixture.Build<MessageInfo>().With(x => x.BookDto, bookdto).With(x => x.ActionType, actionType).Create();

            bookService.Setup(x => x.SaveBook(It.IsAny<MessageInfo>())).ReturnsAsync(true);
            // bookService.Setup(x => x.SaveBook(messageInfo)).ReturnsAsync(true);=> an error occurred all strict...mock
            string message = JsonConvert.SerializeObject(messageInfo);
            var saveData = new SaveData(bookService.Object, logger.Object);
            //Act
            var result = saveData.ExecuteQuery(message);
            //Assert
            Assert.True(result.Result);
            _mockRepository.VerifyAll();
        }
        [Fact]
        public void ExecuteQueryNULL_StateUnderTest_UnExpectedBehavior()
        {
            //Arrange
            Mock<ILogger> logger = new Mock<ILogger>();
            Mock<IBookService> bookService = _mockRepository.Create<IBookService>();
            var saveData = new SaveData(bookService.Object, logger.Object);
            //Act &&                        //Assert
            Assert.ThrowsAsync<ArgumentException>(() => saveData.ExecuteQuery(_fixture.Create<string>()));
            _mockRepository.VerifyAll();
        }
        [Fact]
        public void ExecuteQueryANY_StateUnderTest_UnExpectedBehavior()
        {
            //Arrange
            Mock<ILogger> logger = new Mock<ILogger>();
            Mock<IBookService> bookService = _mockRepository.Create<IBookService>();
            MessageInfo messageInfo = _fixture.Create<MessageInfo>();
            // bookService.Setup(x => x.DeleteBook(bookdto.Id)).ReturnsAsync(true);
            string message = JsonConvert.SerializeObject(messageInfo);
            var saveData = new SaveData(bookService.Object, logger.Object);
            //Act
            var result = saveData.ExecuteQuery(message);
            //Assert
            Assert.False(result.Result);
            _mockRepository.VerifyAll();
        }


    }
}
