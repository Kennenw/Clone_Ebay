using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Clone_Ebay.Models.EBayDB;
using Web_Clone_Ebay.Models.ViewModel;
//using Web_Clone_Ebay.Models;

namespace Web_Clone_Ebay.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly EBayDbContext _context;
        public ProductController(EBayDbContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetProductModels(string? search, int? categoryById, string? sortby, bool isSort = true, int pageIndex = 1, int pageSize = 10)
        {
            if (pageIndex < 1 || pageSize < 1)
            {
                return BadRequest("Page index and size must be positive numbers");
            }

            try
            {
                int skip = (pageIndex - 1) * pageSize;
                var query = _context.Products.AsQueryable();
                if (!string.IsNullOrEmpty(search))
                {
                    search = search.Trim().ToLowerInvariant();
                    query = query.Where(p => (p.Name != null && p.Name.ToLower().Contains(search)) ||
                                             (p.Description != null && p.Description.ToLower().Contains(search)));
                    if (categoryById != null)
                    { query = query.Where(p => p.CategoryId == categoryById); }
                }
                if (categoryById != null)
                {
                    query = query.Where(p => p.CategoryId == categoryById);
                }

                if (!string.IsNullOrEmpty(sortby))
                {
                    query = sortby switch
                    {
                        "Price" => isSort ? query.OrderBy(p => p.Price) : query.OrderByDescending(p => p.Price),
                        "Name" => isSort ? query.OrderBy(p => p.Name) : query.OrderByDescending(p => p.Name),
                        "CreateAt" => isSort ? query.OrderBy(p => p.CreatedAt) : query.OrderByDescending(p => p.CreatedAt),
                        _ => query.OrderBy(p => p.Id)
                    };
                }

                var products = await query
                    .Skip(skip)
                    .Take(pageSize)
                    .Select(p => new ProductViewModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        Stock = p.Stock,
                        CreatedAt = p.CreatedAt,
                        CategoryId = p.CategoryId,
                        CategoryName = p.Category.Name,
                        SellerId = p.SellerId,
                        SellerName = p.Seller.FullName,
                        ImageUrls = p.ProductImages.Select(i => i.ImageUrl).FirstOrDefault() ?? "",
                        AverageRating = p.Ratings.Average(r => r.RatingScore)
                    })
                    .AsNoTracking()
                    .ToListAsync();

                var totalCount = await _context.Products.Where(p => p.Deleted != true).CountAsync();

                return Ok(new PaginatedResult<ProductViewModel>
                {
                    Items = products,
                    TotalCount = totalCount,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving products");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetProductModelById(int id)
        {
            try
            {
                var product = await _context.Products
                    .Where(p => p.Id == id)
                    .Select(p => new ProductDetailResultVM
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Category = p.Category.Name,
                        Description = p.Description ?? "",
                        Price = p.Price,
                        CreatedAt = p.CreatedAt ?? DateTime.UtcNow,
                        ListImage = p.ProductImages != null
                            ? p.ProductImages.Select(img => new ImageUrlVM { ImageUrl = img.ImageUrl }).ToList()
                            : new List<ImageUrlVM>()
                    })
                    .SingleOrDefaultAsync();
                if (product != null)
                {
                    return Ok(product);
                }
                return BadRequest("Do not exits");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving products");
            }
        }

        // [HttpPost("")]
        // public async Task<ActionResult<Product>> PostProductModel(Product model)
        // {
        //     // TODO: Your code here
        //     await Task.Yield();

        //     return null;
        // }

        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutProductModel(int id, Product model)
        // {
        //     // TODO: Your code here
        //     await Task.Yield();

        //     return NoContent();
        // }

        // [HttpDelete("{id}")]
        // public async Task<ActionResult> DeleteProductModelById(int id)
        // {

        //     return null;
        // }
    }
}