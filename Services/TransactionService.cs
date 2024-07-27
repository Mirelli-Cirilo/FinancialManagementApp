using Microsoft.EntityFrameworkCore;
using FinancialManagementApp.Data;
using FinancialManagementApp.Models;
using FinancialManagementApp.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using FinancialManagementApp.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace FinancialManagementApp.Services;

public class TransactionService : ITransactionService
{   
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public TransactionService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<Transaction> AddTransactionAsync(TransactionDto transactionDto, ApplicationUser user)
    {

        var userC = await _userManager.FindByIdAsync(user.Id);
            if (userC == null)
            {
                throw new UnauthorizedAccessException("User is not authorized.");
            }

            var transaction = new Transaction
            {
                Id = 0,
                Description = transactionDto.Description,
                Amount = transactionDto.Amount,
                Date = transactionDto.Date,
                UserId = user.Id
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
            
        
    }

    public async Task<bool> DeleteTransactionAsync(int id)
    {
        var transaction = await _context.Transactions.FindAsync(id);

        if (transaction != null)
        {
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return true;
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
            Description = transaction.Description,
            Amount = transaction.Amount,
            Date = transaction.Date

            
        };
    }

    public async Task<IEnumerable<Transaction>> GetTransactionsAsync(string userId)
    {
        return await _context.Transactions
                             .Where(t => t.UserId == userId)
                             .ToListAsync();
    }

    public async Task UpdateTransactionAsync(Transaction transaction)
    {
        
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();

    }

    public async Task<bool> UpdateTransactionAsync(int id, TransactionDto transactionDto)
    {
        var transaction = await _context.Transactions.FindAsync(id);
        if (transaction == null)
        {
            return false;
        }

        transaction.Description = transactionDto.Description;
        transaction.Amount = transactionDto.Amount;
        transaction.Date = transactionDto.Date;
        
        _context.Transactions.Update(transaction);
        await _context.SaveChangesAsync();
        return true;
    }
}
