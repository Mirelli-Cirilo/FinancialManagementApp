using Microsoft.AspNetCore.Mvc;
using FinancialManagementApp.Models;
using FinancialManagementApp.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using FinancialManagementApp.DTOs;

namespace FinancialManagementApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
    {
        var transactions = await _transactionService.GetTransactionsAsync();
        return Ok(transactions);
    }

    [HttpPost]
    public async Task<ActionResult>CreateTransaction(TransactionDto transactionDto)
    {
        try
            {
                var userClaims = User;
                var transaction = await _transactionService.AddTransactionAsync(transactionDto, userClaims);
                return CreatedAtAction(nameof(GetTransactionById), new { id = transaction.Id }, transaction);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
    }


    [HttpGet("{id}")]
    public async Task<ActionResult>GetTransactionById(int id)
    {
        var transaction = await _transactionService.GetTransactionById(id);
        if (transaction == null)
        {
            return NotFound();
        }
        return Ok(transaction);
        
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult>DeleteTransaction(int id)
    {
        await _transactionService.DeleteTransactionAsync(id);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult>UpdateTransaction(TransactionDto transactionDto, int id)
    {

        var result = await _transactionService.UpdateTransactionAsync(id, transactionDto);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }


}
