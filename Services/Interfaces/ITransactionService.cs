using FinancialManagementApp.Models;
using System.Security.Claims;
using FinancialManagementApp.DTOs;

namespace FinancialManagementApp.Services.Interfaces;

public interface ITransactionService
{
    Task<IEnumerable<Transaction>> GetTransactionsAsync();
    Task<TransactionDto> GetTransactionById(int id);
    Task<Transaction> AddTransactionAsync(TransactionDto transactionDto, ClaimsPrincipal userClaims);
    Task<bool> DeleteTransactionAsync(int id);
    Task<bool> UpdateTransactionAsync(int id, TransactionDto transactionDto);

}
