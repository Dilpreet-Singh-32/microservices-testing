using Catalog.API.Entities;
using Catalog.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IProductRepository repository, ILogger<CatalogController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Get -> all products
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _repository.GetProducts());
        }

        // Get -> single product
        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        public async Task<IActionResult> GetProduct(string id)
        {
            var product = await _repository.GetProduct(id);

            if (product == null)
            {
                _logger.LogError($"Product with id: {id}, not found.");
                return NotFound();
            }

            return Ok(product);
        }

        [Route("[action]/{name}", Name = "GetProductsByName")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByName(string name)
        {
            var items = await _repository.GetProductsByName(name);
            if (items == null)
            {
                _logger.LogError($"Products with name: {name} not found.");
                return NotFound();
            }

            return Ok(items);
        }

        [Route("[action]/{category}", Name = "GetProductsByCategory")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(string category)
        {
            var items = await _repository.GetProductsByCategory(category);
            if (items == null)
            {
                _logger.LogError($"Products with category: {category} not found.");
                return NotFound();
            }

            return Ok(items);
        }

        [HttpPost()]
        public async Task<ActionResult<Product>> Post(Product product)
        {
            await _repository.CreateProduct(product);

            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateProduct(string id, [FromBody] Product product)
        {
            var productExists = await _repository.GetProduct(id) != null;

            if (!productExists)
            {
                return NotFound();
            }

            return Ok(await _repository.UpdateProduct(product));
        }

        [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
        public async Task<IActionResult> DeleteProductById(string id)
        {
            return Ok(await _repository.DeleteProduct(id));
        }
    }
}
