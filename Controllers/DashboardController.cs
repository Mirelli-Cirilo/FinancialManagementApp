using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FinancialManagementApp.Data;

namespace FinancialManagementApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactionData()
        {
            var data = await _context.Transactions
                .GroupBy(t => new { t.Date.Date, t.Description })  // Group by Date and Description
                .Select(g => new 
                {
                    Date = g.Key.Date,
                    Description = g.Key.Description,
                    TotalAmount = g.Sum(t => (double)t.Amount)  // Convert to double
                })
                .ToListAsync();

            return Ok(data);
        }
    }
}