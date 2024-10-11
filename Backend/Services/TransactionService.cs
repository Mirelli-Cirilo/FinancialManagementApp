using Microsoft.EntityFrameworkCore;
using FinancialManagementApp.Data;
using FinancialManagementApp.Models;
using FinancialManagementApp.Services.Interfaces;
using FinancialManagementApp.DTOs;
using FinancialManagementApp.Mappers;


namespace FinancialManagementApp.Services;

public class TransactionService : ITransactionService
{   
    private readonly ApplicationDbContext _context;


    public TransactionService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TransactionDto> AddTransactionAsync(CreateTransactionDto transactionDto, string userId)
    {
        if (userId == null)
        {
            throw new UnauthorizedAccessException("User is not authorized.");
        }

        var budget = _context.Budgets.FirstOrDefault(b => b.Id == transactionDto.BudgetId);

        if (budget == null)
        {
            throw new KeyNotFoundException($"Budget with Id not found.");
        }
    
        var transactionModel = transactionDto.ToTransactionFromCreateDto();
        budget.Amount -= transactionModel.Amount; 
   
        if (budget.Amount < 0) { budget.Amount = 0; }

        _context.Transactions.Add(transactionModel);
        await _context.SaveChangesAsync();

        return transactionModel.ToTransactionDto();
    }

    public async Task<TransactionDto> GetTransactionById(int id)
    {
        var transaction = await _context.Transactions.FirstOrDefaultAsync(b => b.Id == id);

        return transaction.ToTransactionDto();
    }

    public async Task<IEnumerable<TransactionDto>> GetTransactionsAsync(string userId)
    {
        var transactions = await _context.Transactions
                         .Where(t => t.UserId == userId)
                         .OrderByDescending(t => t.createdAt)
                         .Select(t => t.ToTransactionDto()) 
                         .ToListAsync();

        return transactions;

    }

    public async Task<TransactionDto> UpdateTransactionAsync(int id, UpdateTransactionDto transactionDto)
    {
        var transaction = _context.Transactions.FirstOrDefault(x => x.Id == id);

        transaction.Amount = transactionDto.Amount;
        transaction.Title = transactionDto.Title;
        transaction.createdAt = transactionDto.CreatedAt;
        
        _context.Transactions.Update(transaction);
        await _context.SaveChangesAsync();

        return transaction.ToTransactionDto();
    }

    public async Task<bool> DeleteTransactionAsync(int id)
    {
        var transaction = _context.Transactions.FirstOrDefault(x => x.Id == id);

        if (transaction != null)
        {
            var budget = await _context.Budgets.FindAsync(transaction.BudgetId);

            if (budget != null)
            {
                budget.Amount += transaction.Amount;

                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();
                return true;
            }
        }
        return false;
    }
}
