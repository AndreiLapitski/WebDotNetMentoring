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

        // GET /api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
        {
            IEnumerable<Product> products = await _productRepository.GetAll().ToListAsync();
            return _mapper.Map<List<ProductDto>>(products);
        }

        // POST /api/Products
        [HttpPost]
        public async Task<ActionResult<Product>> Create(Product product)
        {
            product.ProductId = await _productRepository.CreateAsync(product);
            return product;
        }

        // DELETE /api/Products/5
        [HttpDelete("id")]
        public async Task<ActionResult> Delete(int id)
        {
            Product product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            await _productRepository.DeleteAsync(product);
            return NoContent();
        }

        // PUT /api/Products/3
        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> Update(int id, Product updatedProduct)
        {
            if (id != updatedProduct.ProductId)
            {
                return BadRequest();
            }

            await _productRepository.UpdateAsync(id, updatedProduct);
            return NoContent();
        }
    }
}