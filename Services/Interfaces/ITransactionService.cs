using FinancialManagementApp.Models;
using FinancialManagementApp.DTOs;

namespace FinancialManagementApp.Services.Interfaces;

public interface ITransactionService
{
    Task<IEnumerable<TransactionDto>> GetTransactionsAsync(string userId);
    Task<TransactionDto> GetTransactionById(int id);
    Task<Transaction> AddTransactionAsync(TransactionDto transactionDto, string userId);
    Task<bool> DeleteTransactionAsync(int id);
    Task<bool> UpdateTransactionAsync(int id, TransactionDto transactionDto);

}
