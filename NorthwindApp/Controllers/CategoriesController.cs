using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthwindApp.DTO;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
        {
            IEnumerable<Category> categories = await _categoryRepository.GetAll().ToListAsync();
            return _mapper.Map<List<CategoryDto>>(categories);
        }
    }
}
