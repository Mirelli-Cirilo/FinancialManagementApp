using Microsoft.AspNetCore.Mvc;
using FinancialManagementApp.Models;
using FinancialManagementApp.Services.Interfaces;
using System.Security.Claims;
using FinancialManagementApp.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FinancialManagementApp.DTOs;

namespace FinancialManagementApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BudgetController : ControllerBase
{
    private readonly IBudgetService _budgetService;
    private readonly ApplicationDbContext _context;

    public BudgetController(IBudgetService budgetService, ApplicationDbContext context)
    {
        _budgetService = budgetService;
        _context = context;
    }

    [HttpGet("budgetExist")]
    public async Task<ActionResult<bool>> BudgetExist()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var budgetExist = await _context.Budgets.AnyAsync(b => b.UserId == userId);
        return Ok(budgetExist);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Budget>>>GetBudgets()
    {
        var budgets = await _budgetService.GetBudgetsAsync();
        return Ok(budgets);
    }

    [HttpPost("create")]
    public async Task<ActionResult> CreateBudget([FromBody] Budget budget)
{
    // O UserId agora já faz parte do objeto budget enviado do front-end
    var newBudget = await _budgetService.AddBudgetAsync(budget);

    // Retorna o orçamento criado com o status 201 Created
    return CreatedAtAction(nameof(GetBudgetById), new { id = newBudget.Id }, newBudget);
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
    public async Task<ActionResult> UpdateBudget(int id, [FromBody] BudgetDetailsDto budget)
    {
        if (id != budget.Id)
        {
            return BadRequest("Id does not match");
        }

        await _budgetService.UpdateBudgetAsync(id, budget);
        return Ok();
    }
}
