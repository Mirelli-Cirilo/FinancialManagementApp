using FinancialManagementApp.DTOs;
using FinancialManagementApp.Models;

namespace FinancialManagementApp.Mappers
{
    public static class TransactionMappers
    {
        public static TransactionDto ToTransactionDto(this Transaction transaction)
        {
            return new TransactionDto
            {
                Id = transaction.Id,
                Title = transaction.Title,
                Amount = transaction.Amount,
                CreatedAt = transaction.createdAt,
                BudgetId = transaction.BudgetId,
                UserId = transaction.UserId
            };
        }

        public static Transaction ToTransactionFromCreateDto(this CreateTransactionDto transactionDto)
        {
            return new Transaction
            {
                Id = transactionDto.Id,
                Title = transactionDto.Title,
                Amount = transactionDto.Amount,
                createdAt = transactionDto.CreatedAt,
                BudgetId = transactionDto.BudgetId,
                Budget = transactionDto.Budget,
                UserId = transactionDto.UserId
            };
        }

        public static Transaction ToTransactionFromUpdateDto(this UpdateTransactionDto transactionDto)
        {
            return new Transaction
            {
                Title = transactionDto.Title,
                Amount = transactionDto.Amount,
                createdAt = transactionDto.CreatedAt,
                BudgetId = transactionDto.BudgetId,
                Budget = transactionDto.Budget,
                UserId = transactionDto.UserId
            };
        }
    }
}
