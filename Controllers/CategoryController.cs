using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Clone_Ebay.Models.EBayDB;
//using Web_Clone_Ebay.Models;

namespace Web_Clone_Ebay.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly EBayDbContext _context;
        public CategoryController(EBayDbContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetCategoryModels()
        {
            try
            {
                var categories = await _context.Categories
                    .AsNoTracking()
                    .Select( c => new CategoryViewModel{
                        Id = c.Id,
                        Name = c.Name,
                    })
                    .ToListAsync();
                if (categories != null)
                {
                    return Ok(categories);
                }
                return BadRequest("Do not exits");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving categories");
            }
        }

        // [HttpGet("{id}")]
        // public async Task<ActionResult> GetCategoryModelById(int id)
        // {
        //     return null;
        // }

        // [HttpPost("")]
        // public async Task<ActionResult<TModel>> PostTModel(TModel model)
        // {
        //     // TODO: Your code here
        //     await Task.Yield();

        //     return null;
        // }

        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutTModel(int id, TModel model)
        // {
        //     // TODO: Your code here
        //     await Task.Yield();

        //     return NoContent();
        // }

        // [HttpDelete("{id}")]
        // public async Task<ActionResult<TModel>> DeleteTModelById(int id)
        // {
        //     // TODO: Your code here
        //     await Task.Yield();

        //     return null;
        // }
    }
}