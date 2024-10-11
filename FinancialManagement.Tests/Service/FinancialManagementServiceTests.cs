using FinancialManagementApp.Data;
using FinancialManagementApp.DTOs;
using FinancialManagementApp.Models;
using FinancialManagementApp.Services;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace FinancialManagement.Tests.Service
{
    public class FinancialManagementServiceTests
    {
        private async Task<ApplicationDbContext> GetDatabaseContext()
        {
          
            var connection = new SqliteConnection("Filename=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            var context = new ApplicationDbContext(options);
            await context.Database.EnsureCreatedAsync();

            if(await context.Transactions.CountAsync() <= 0)
{
                var budget1 = new Budget() { Title = "Family Budget", Amount = 5000, UserId = "1" };
                var budget2 = new Budget() { Title = "Personal Budget", Amount = 3000, UserId = "2" };

                // Criar várias transações
                for (int i = 1; i <= 10; i++)
                {
                    var transaction = new Transaction()
                    {
                        Title = $"Transaction {i}",
                        Amount = i * 100, 
                        UserId = $"User{i}",
                        createdAt = DateTime.UtcNow.AddDays(-i), 
                        Budget = (i % 2 == 0) ? budget1 : budget2 
                    };

                    context.Transactions.Add(transaction);
                }

                await context.SaveChangesAsync(); 
            }

            return context;
        }


        [Fact]
        public async void FinancialManagement_GetTransactions_ReturnTransactions()
        {
            var userId = "1";
            var dbContext = await GetDatabaseContext();
            var transactionService = new TransactionService(dbContext);

            var result = await transactionService.GetTransactionsAsync(userId);

            result.Should().NotBeNull();
            result.Should().BeOfType<List<TransactionDto>>();
        }

        [Fact]
        public async void FinancialManagement_GetTransactionById_ReturnTransaction()
        {
            var id = 1;
            var dbContext = await GetDatabaseContext();
            var transactionService = new TransactionService(dbContext);

            var result = await transactionService.GetTransactionById(id);

            result.Should().NotBeNull();
            result.Should().BeOfType<TransactionDto>();
        }

        [Fact]
        public async void FinancialManagement_AddTransaction_ReturnCreatedTransaction()
        {
            var transactionDto = new CreateTransactionDto
            {
                // Inicializa com dados válidos
                Title = "Test Transaction",
                Amount = 50,
                BudgetId = 1,
                UserId = "1"
            };
            var userId = "1";
            var dbContext = await GetDatabaseContext();
            var transactionService = new TransactionService(dbContext);

            var result = await transactionService.AddTransactionAsync(transactionDto, userId);

            result.Should().NotBeNull();
            result.Should().BeOfType<TransactionDto>();
        }

        [Fact]
        public async void FinancialManagement_UpdateTransaction_ReturnUpdatedTransaction()
        {
            var userId = "1"; // Defina o ID do usuário
            var initialTransaction = new Transaction
            {
                Title = "Initial Transaction",
                Amount = 30,
                UserId = userId, // Aqui você define o UserId
                BudgetId = 1
            };

            var dbContext = await GetDatabaseContext();
            dbContext.Transactions.Add(initialTransaction);
            await dbContext.SaveChangesAsync(); // Salve a transação inicial

            var transactionDto = new UpdateTransactionDto
            {
                Title = "Updated Transaction",
                Amount = 50,
                BudgetId = 1,
                CreatedAt = DateTime.UtcNow // Certifique-se de que este valor está correto
            };

            var id = initialTransaction.Id; // Obtenha o ID da transação inicial
            var transactionService = new TransactionService(dbContext);

            var result = await transactionService.UpdateTransactionAsync(id, transactionDto);

            result.Should().NotBeNull();
            result.Should().BeOfType<TransactionDto>();
        }

        [Fact]
        public async void FinancialManagement_DeleteTransaction_ReturnDelete()
        {
            var id = 1;
            var dbContext = await GetDatabaseContext();
            var transactionService = new TransactionService(dbContext);

            var result = await transactionService.DeleteTransactionAsync(id);

            result.Should().BeTrue();
           
        }
    }
}
