using FinancialManagementApp.Models;
using FinancialManagementApp.DTOs;

namespace FinancialManagementApp.Services.Interfaces;

public interface ITransactionService
{
    Task<IEnumerable<TransactionDto>> GetTransactionsAsync(string userId);
    Task<TransactionDto> GetTransactionById(int id);
    Task<TransactionDto> AddTransactionAsync(CreateTransactionDto transactionDto, string userId);
    Task<bool> DeleteTransactionAsync(int id);
    Task<TransactionDto> UpdateTransactionAsync(int id, UpdateTransactionDto transactionDto);

}
