using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthwindApp.DTO;
using NorthwindApp.Helpers;
using NorthwindApp.Interfaces;
using NorthwindApp.Models;

namespace NorthwindApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Category> _categoryRepository;

        public CategoriesController(IRepository<Category> categoryRepository, IMapper mapper)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        // GET /api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
        {
            IEnumerable<Category> categories = await _categoryRepository.GetAll().ToListAsync();
            return _mapper.Map<List<CategoryDto>>(categories);
        }

        // GET /api/Categories/4
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryImage(int id)
        {
            Category category = await _categoryRepository.GetByIdAsync(id);
            if (category?.Picture == null || !category.Picture.Any())
            {
                return NotFound();
            }

            return File(
                category.Picture,
                Constants.ContentTypeDownload,
                $"{category.CategoryName}.jpg");
        }

        // PUT /api/Categories/3
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategoryImage(int id)
        {
            Category category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            await using MemoryStream stream = new MemoryStream();
            await Request.Body.CopyToAsync(stream);
            byte[] bytes = stream.ToArray();

            if (!bytes.Any())
            {
                return BadRequest("No picture");
            }

            category.Picture = bytes;
            await _categoryRepository.SaveAsync();

            return Ok();
        }
    }
}
