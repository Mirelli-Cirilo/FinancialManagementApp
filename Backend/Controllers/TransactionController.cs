using Microsoft.AspNetCore.Mvc;
using FinancialManagementApp.Services.Interfaces;
using FinancialManagementApp.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FinancialManagementApp.Controllers;

[Authorize]
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
    public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactions()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var transactions = await _transactionService.GetTransactionsAsync(userId);
        if (transactions == null)
        {
            return NotFound();
        }
        return Ok(transactions);
    }

    [HttpPost]
    public async Task<ActionResult>CreateTransaction([FromBody] CreateTransactionDto transactionDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
        var transaction = await _transactionService.AddTransactionAsync(transactionDto, userId);

        return CreatedAtAction(nameof(GetTransactionById), new { id = transaction.Id }, transaction);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult>GetTransactionById([FromRoute] int id)
    {
        var transaction = await _transactionService.GetTransactionById(id);
        if (transaction == null)
        {
            return NotFound();
        }
        return Ok(transaction);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult>UpdateTransaction([FromBody] UpdateTransactionDto transactionDto, [FromRoute] int id)
    {
        var transaction = await _transactionService.UpdateTransactionAsync(id, transactionDto);
        if (transaction == null)
        {
            return NotFound();
        }
        return Ok(transaction);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTransaction([FromRoute] int id)
    {
        var transaction = await _transactionService.DeleteTransactionAsync(id);
        if (transaction == false)
        {
            return NotFound();
        }
        return NoContent();
    }

}
