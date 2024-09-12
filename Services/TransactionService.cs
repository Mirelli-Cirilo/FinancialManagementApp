using Microsoft.EntityFrameworkCore;
using FinancialManagementApp.Data;
using FinancialManagementApp.Models;
using FinancialManagementApp.Services.Interfaces;
using FinancialManagementApp.DTOs;


namespace FinancialManagementApp.Services;

public class TransactionService : ITransactionService
{   
    private readonly ApplicationDbContext _context;


    public TransactionService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Transaction> AddTransactionAsync(TransactionDto transactionDto, string userId)
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
    
    var transaction = new Transaction
    {
        Id = 0,
        Title = transactionDto.Title,
    
        Amount = transactionDto.Amount,
       
        UserId = userId,
        BudgetId = transactionDto.BudgetId,
        Budget = budget
    };

    budget.Amount -= transactionDto.Amount; 
   
    if (budget.Amount < 0)
    {
        budget.Amount = 0;
    }

    try
    {
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception: {ex.Message}");
        throw;
    }

    return transaction;
}

    public async Task<bool> DeleteTransactionAsync(int id)
    {
        var transaction = await _context.Transactions.FindAsync(id);
        

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

    public async Task<TransactionDto> GetTransactionById(int id)
    {
        var transaction = _context.Transactions.FirstOrDefault(p => p.Id == id);

        if (transaction == null)
        {
            return null; 
        }

        return new TransactionDto
        {
            Id = transaction.Id,      
            Title = transaction.Title,
            Amount = transaction.Amount,
            CreatedAt = transaction.createdAt
        };
    }

    public async Task<IEnumerable<TransactionDto>> GetTransactionsAsync(string userId)
    {
        var transactions = await _context.Transactions
                             .Where(t => t.UserId == userId).OrderByDescending(t => t.createdAt)
                             .ToListAsync();


        var result = transactions.Select(t => new TransactionDto
        {
            Id = t.Id,
            Title = t.Title,
            Amount = t.Amount,
            CreatedAt = t.createdAt,
            BudgetId = t.BudgetId
        });

        return result;

    }



    public async Task<bool> UpdateTransactionAsync(int id, TransactionDto transactionDto)
    {
        var transaction = await _context.Transactions.FindAsync(id);
        if (transaction == null)
        {
            return false;
        }

        transaction.Amount = transactionDto.Amount;
        
        _context.Transactions.Update(transaction);
        await _context.SaveChangesAsync();
        return true;
    }
}
