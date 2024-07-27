using FinancialManagementApp.Models;
using System.Security.Claims;
using FinancialManagementApp.DTOs;

namespace FinancialManagementApp.Services.Interfaces;

public interface ITransactionService
{
    Task<IEnumerable<Transaction>> GetTransactionsAsync(string userId);
    Task<TransactionDto> GetTransactionById(int id);
    Task<Transaction> AddTransactionAsync(TransactionDto transactionDto, ApplicationUser user);
    Task<bool> DeleteTransactionAsync(int id);
    Task<bool> UpdateTransactionAsync(int id, TransactionDto transactionDto);

}
