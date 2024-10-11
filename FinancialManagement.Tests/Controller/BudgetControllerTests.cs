using FakeItEasy;
using FinancialManagementApp.Controllers;
using FinancialManagementApp.DTOs;
using FinancialManagementApp.Models;
using FinancialManagementApp.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace FinancialManagement.Tests.Controller
{
    public class BudgetControllerTests
    {
        private readonly IBudgetService _fakeBudgetService;
        private readonly BudgetController _controller;

        public BudgetControllerTests()
        {
          
            _fakeBudgetService = A.Fake<IBudgetService>();

            _controller = new BudgetController(_fakeBudgetService);
        }

        [Fact]
        public async Task BudgetController_BudgetExist_ReturnOk()
        {
            // Arrange
            

            A.CallTo(() => _fakeBudgetService.BudgetsExist())
                .Returns(true);

            // Act
            var result = await _controller.BudgetExist();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(true);
        }

        [Fact]
        public async Task BudgetController_GetBudget_ReturnOk()
        {
            // Arrange
            var fakeBudgetsDtos = new List<BudgetDetailsDto>
        {
            new BudgetDetailsDto { Id = 1, Title = "Budget1", InitialAmount = 100 },
            new BudgetDetailsDto { Id = 2, Title = "Budget2", InitialAmount = 200 }
        };

            A.CallTo(() => _fakeBudgetService.GetBudgetsAsync())
                .Returns(Task.FromResult<IEnumerable<BudgetDetailsDto>>(fakeBudgetsDtos)); 

            // Act
            var result = await _controller.GetBudgets();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200); 
            okResult.Value.Should().BeEquivalentTo(fakeBudgetsDtos);
        }

        [Fact]
        public async Task BudgetController_CreateBudget_ReturnCreated()
        {
            // Arrange
           
            var fakeBudgetDto = new CreateBudgetDto {Title = "Budget",  InitialAmount = 100 };
            var fakeBudget = new BudgetDetailsDto { Id = 1, InitialAmount = 100 };

          
            A.CallTo(() => _fakeBudgetService.AddBudgetAsync(fakeBudgetDto))
                .Returns(Task.FromResult(fakeBudget));

            // Act
            var result = await _controller.CreateBudget(fakeBudgetDto);

            // Assert
            var createdAtActionResult = result as CreatedAtActionResult;
            createdAtActionResult.Should().NotBeNull();
            createdAtActionResult.StatusCode.Should().Be(201); // Created
            createdAtActionResult.Value.Should().BeEquivalentTo(fakeBudget);
        }

        [Fact]
        public async Task TransactionController_GetTransactionById_ReturnOk()
        {
            // Arrange
            var budgetId = 1;
            var fakeBudgetDto = new BudgetDetailsDto
            {
                Id = budgetId,
                Title = "Test Transaction",
                InitialAmount = 100
            };

            A.CallTo(() => _fakeBudgetService.GetBudgetByIdAsync(budgetId))
                .Returns(Task.FromResult(fakeBudgetDto));

            // Act
            var result = await _controller.GetBudgetById(budgetId);

            // Assert
            result.Should().NotBeNull();

            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();          
            okResult.StatusCode.Should().Be(200);    
            okResult.Value.Should().BeEquivalentTo(fakeBudgetDto);
        }

        

        [Fact]
        public async Task BudgetController_UpdateBudget_ReturnOk()
        {
            // Arrange
            var budgetId = 1;
            var fakebudgetDto = new UpdateBudgetDto
            {
                Title = "Test Transaction",
                InitialAmount = 100
            };

            var fakeUpdatedBudget = new BudgetDetailsDto
            {
                Id = budgetId,
                Title = "Test Transaction",
                InitialAmount = 100

            };

       
            A.CallTo(() => _fakeBudgetService.UpdateBudgetAsync(budgetId, fakebudgetDto))
                .Returns(Task.FromResult(fakeUpdatedBudget));

            // Act
            var result = await _controller.UpdateBudget(budgetId, fakebudgetDto);

            // Assert
            result.Should().NotBeNull();

            var okResult = result as OkObjectResult; 
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);    
            okResult.Value.Should().BeEquivalentTo(fakeUpdatedBudget);

        }

        [Fact]
        public async Task BudgetnController_DeleteBudget_ReturnOk()
        {
            // Arrange
            var budgetId = 1;
           
            A.CallTo(() => _fakeBudgetService.DeleteBudgetAsync(budgetId))
                .Returns(Task.FromResult(true));
            // Act
            var result = await _controller.DeleteBudget(budgetId);

            // Assert

            result.Should().NotBeNull();

           
            var noContentResult = result as NoContentResult;
            noContentResult.Should().NotBeNull();
            noContentResult.StatusCode.Should().Be(204); 

           
            A.CallTo(() => _fakeBudgetService.DeleteBudgetAsync(budgetId))
                .MustHaveHappenedOnceExactly();  
        }

    }
}
