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
    public class ProductsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Product> _productRepository;

        public ProductsController(IRepository<Product> productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
        {
            IEnumerable<Product> products = await _productRepository.GetAll().ToListAsync();
            return _mapper.Map<List<ProductDto>>(products);
        }
    }
}