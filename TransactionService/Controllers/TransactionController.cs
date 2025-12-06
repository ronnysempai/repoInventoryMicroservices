using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransactionService.Data;
using TransactionService.Models;

namespace TransactionService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly TransactionDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public TransactionController(TransactionDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Transaction t)
        {
            // VALIDAR PRODUCTO CON PRODUCTSERVICE
            var client = _httpClientFactory.CreateClient("ProductService");

            var productResponse = await client.GetAsync($"/api/products/{t.ProductId}");

            if (!productResponse.IsSuccessStatusCode)
                return BadRequest("Product does not exist.");

            // GUARDAR TRANSACCIÓN
            _context.Transactions.Add(t);
            await _context.SaveChangesAsync();

            return Ok(t);
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetByProduct(int productId)
        {
            var result = await _context
                .Transactions
                .Where(x => x.ProductId == productId)
                .ToListAsync();

            return Ok(result);
        }
    }
}
