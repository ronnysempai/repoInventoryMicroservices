using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.DTOs;
using ProductService.Models;
using ProductService.Repositories;
using System;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repo;
        private readonly ProductDbContext _context;


        public ProductsController(IProductRepository repo, ProductDbContext context)
        {
            _repo = repo;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? name, [FromQuery] string? category)
        {
            var products = await _repo.GetAllAsync(name, category);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _repo.GetByIdAsync(id);
            return product == null ? NotFound() : Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductCreateDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Category = dto.Category,
                ImageUrl = dto.ImageUrl,
                Price = dto.Price,
                Stock = dto.Stock
            };

            var created = await _repo.CreateAsync(product);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product updatedProduct)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound("Product not found");

            product.Name = updatedProduct.Name;
            product.Description = updatedProduct.Description;
            product.Category = updatedProduct.Category;
            product.Price = updatedProduct.Price;
            product.Stock = updatedProduct.Stock;
            product.ImageUrl = updatedProduct.ImageUrl;

            await _context.SaveChangesAsync();

            return Ok(product);
        }


        

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound("Product not found");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Product deleted successfully" });
        }

    }
}
