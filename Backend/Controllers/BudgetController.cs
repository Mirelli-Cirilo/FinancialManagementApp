using Microsoft.AspNetCore.Mvc;
using FinancialManagementApp.Services.Interfaces;
using System.Security.Claims;
using FinancialManagementApp.Data;
using Microsoft.EntityFrameworkCore;
using FinancialManagementApp.DTOs;

namespace FinancialManagementApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BudgetController : ControllerBase
{
    private readonly IBudgetService _budgetService;
    private readonly ApplicationDbContext _context;

    public BudgetController(IBudgetService budgetService)
    {
        _budgetService = budgetService;
    }

    [HttpGet("budgetExist")]
    public async Task<ActionResult<bool>> BudgetExist()
    {

        var budgetExist = await _budgetService.BudgetsExist();
        return Ok(budgetExist);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BudgetDetailsDto>>>GetBudgets()
    {
        var budgets = await _budgetService.GetBudgetsAsync();
        if (budgets == null)
        {
            return NotFound();
        }
        return Ok(budgets);
    }

    [HttpPost("create")]
    public async Task<ActionResult> CreateBudget([FromBody] CreateBudgetDto budget)
    {
    
        var newBudget = await _budgetService.AddBudgetAsync(budget);

        return CreatedAtAction(nameof(GetBudgetById), new { id = newBudget.Id }, newBudget);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BudgetDetailsDto>> GetBudgetById([FromRoute] int id)
    {
        var budget = await _budgetService.GetBudgetByIdAsync(id);
        if (budget == null)
        {
            return NotFound();
        }
        return Ok(budget);
    }


    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateBudget([FromRoute] int id, [FromBody] UpdateBudgetDto budgetDto)
    {
        var budget = await _budgetService.UpdateBudgetAsync(id, budgetDto);
        if (budget == null)
        {
            return NotFound();
        }
        return Ok(budget);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteBudget([FromRoute] int id)
    {
        var budget = await _budgetService.DeleteBudgetAsync(id);

        if (budget == false)
        {
            return NotFound();
        }
        return NoContent();
    }
}