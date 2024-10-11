using FakeItEasy;
using FinancialManagementApp.Controllers;
using FinancialManagementApp.DTOs;
using FinancialManagementApp.Models;
using FinancialManagementApp.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinancialManagement.Tests.Controller
{
    public class TransactionControllerTest
    {
        private readonly ITransactionService _fakeTransactionService;
        private readonly TransactionController _controller;

        public TransactionControllerTest()
        {
            _fakeTransactionService = A.Fake<ITransactionService>();
            _controller = new TransactionController(_fakeTransactionService);

            var fakeUserId = "user123";
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
            new Claim(ClaimTypes.NameIdentifier, fakeUserId)
            }, "mock"));

            var context = A.Fake<HttpContext>();
            A.CallTo(() => context.User).Returns(user);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = context
            };
        }

        [Fact]
        public async Task TransactionController_GetTransactions_ReturnOk()
        {
            // Arrange
            var fakeTransactionDtos = new List<TransactionDto>
            {
                new TransactionDto { Id = 1, Amount = 100 },
                new TransactionDto { Id = 2, Amount = 200}
            };

            A.CallTo(() => _fakeTransactionService.GetTransactionsAsync(A<string>.Ignored))
                .Returns(Task.FromResult<IEnumerable<TransactionDto>>(fakeTransactionDtos));

            // Act
            var result = await _controller.GetTransactions();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200); 
            okResult.Value.Should().BeEquivalentTo(fakeTransactionDtos);
        }

        [Fact]
        public async Task TransactionController_CreateTransaction_ReturnCreated()
        {
            // Arrange
            var fakeTransactionDto = new CreateTransactionDto { Amount = 100, BudgetId = 1 };
            var fakeTransaction = new TransactionDto { Id = 1, Amount = 100 };

            
            A.CallTo(() => _fakeTransactionService.AddTransactionAsync(fakeTransactionDto, A<string>.Ignored))
                .Returns(Task.FromResult(fakeTransaction));

            // Act
            var result = await _controller.CreateTransaction(fakeTransactionDto);

            // Assert
            var createdAtActionResult = result as CreatedAtActionResult;
            createdAtActionResult.Should().NotBeNull();
            createdAtActionResult.StatusCode.Should().Be(201); 
            createdAtActionResult.Value.Should().BeEquivalentTo(fakeTransaction);
        }

        [Fact]
        public async Task TransactionController_GetTransactionById_ReturnOk()
        {
            // Arrange
            var transactionId = 1;
            var fakeTransactionDto = new TransactionDto
            {
                Id = transactionId,
                Title = "Test Transaction",
                Amount = 100,
                CreatedAt = DateTime.Now
            };

            
            A.CallTo(() => _fakeTransactionService.GetTransactionById(transactionId))
                .Returns(Task.FromResult(fakeTransactionDto));

            // Act
            var result = await _controller.GetTransactionById(transactionId);

            // Assert
            result.Should().NotBeNull();

            var okResult = result as OkObjectResult; 
            okResult.Should().NotBeNull();           
            okResult.StatusCode.Should().Be(200);    
            okResult.Value.Should().BeEquivalentTo(fakeTransactionDto);
        }

        [Fact]
        public async Task TransactionController_UpdateTransaction_ReturnOk()
        {
            // Arrange
            var transactionId = 1;
            var fakeTransactionDto = new UpdateTransactionDto
            {
                Title = "Test Transaction",
                Amount = 100,
                CreatedAt = DateTime.Now
            };

            var fakeUpdatedTransaction = new TransactionDto
            {
                Id = transactionId,
                Title = "Test Transaction",
                Amount = 100,
                CreatedAt = DateTime.Now
            };

            A.CallTo(() => _fakeTransactionService.UpdateTransactionAsync(transactionId, fakeTransactionDto))
                .Returns(Task.FromResult(fakeUpdatedTransaction));


            // Act
            var result = await _controller.UpdateTransaction(fakeTransactionDto, transactionId);

            // Assert
            

            var okResult = result as OkObjectResult; 
            okResult.Should().NotBeNull();          
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(fakeUpdatedTransaction);
        }

        [Fact]
        public async Task TransactionController_DeleteTransaction_ReturnOk()
        {
            // Arrange
            var transactionId = 1;

            A.CallTo(() => _fakeTransactionService.DeleteTransactionAsync(transactionId))
                .Returns(Task.FromResult(true)); ;


            // Act
            var result = await _controller.DeleteTransaction(transactionId);

            // Assert

            result.Should().NotBeNull();

            var noContentResult = result as NoContentResult; 
            noContentResult.Should().NotBeNull(); 
            noContentResult.StatusCode.Should().Be(204); 

            A.CallTo(() => _fakeTransactionService.DeleteTransactionAsync(transactionId))
                .MustHaveHappenedOnceExactly();
        }
    }
}
