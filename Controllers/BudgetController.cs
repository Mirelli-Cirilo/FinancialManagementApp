using Microsoft.AspNetCore.Mvc;
using FinancialManagementApp.Models;
using FinancialManagementApp.Services.Interfaces;

namespace FinancialManagementApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BudgetController : ControllerBase
{
    private readonly IBudgetService _budgetService;

    public BudgetController(IBudgetService budgetService)
    {
        _budgetService = budgetService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Budget>>>GetBudgets()
    {
        var budgets = await _budgetService.GetBudgetsAsync();
        return Ok(budgets);
    }

    [HttpPost]
    public async Task<ActionResult>CreateBudget(Budget budget)
    {
        await _budgetService.AddBudgetAsync(budget);
        return CreatedAtAction(nameof(GetBudgetById), new{id = budget.Id}, budget);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Budget>> GetBudgetById(int id)
    {
        var budget = await _budgetService.GetBudgetByIdAsync(id);
        if (budget == null)
        {
            return NotFound("Budget not found");
        }
        return Ok(budget);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult>DeleteBudget(int id)
    {
        await _budgetService.DeleteBudgetAsync(id);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult>UpdateBudget(Budget budget, int id)
    {
        if (id != budget.Id)
        {
            return BadRequest("Id Does not match");
        }

        var existBudget = _budgetService.GetBudgetByIdAsync(id);
        if (existBudget != null)
        {
            return NotFound("Budget not Found");
        }

        await _budgetService.UpdateBudgetAsync(budget);
        return Ok();
    }
}
