using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.API.Data.Interfaces;
using Catalog.API.Entities;
using Catalog.API.Repositories.Interfaces;
using System;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public readonly ICatalogContext _context;

        public ProductRepository(ICatalogContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products
                .Find(p => true)
                .ToListAsync();
        }

        public async Task<Product> GetProduct(string id)
        {
            return await _context.Products
                .Find(p => p.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByName(string name)
        {
            FilterDefinition<Product> filters = Builders<Product>.Filter.Eq(p => p.Name, name);

            return await _context.Products.Find(filters).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategory(string categoryName)
        {
            FilterDefinition<Product> filters = Builders<Product>.Filter.Eq(p => p.Category, categoryName);

            return await _context.Products.Find(filters).ToListAsync();
        }

        public async Task CreateProduct(Product product)
        {
            await _context.Products.InsertOneAsync(product);
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updateResult = await _context.Products
                .ReplaceOneAsync(filter: p => p.Id == product.Id, replacement: product);

            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> DeleteProduct(string id)
        {
            FilterDefinition<Product> filters = Builders<Product>.Filter.Eq(p => p.Id, id);

            var deleteResult = await _context.Products.DeleteOneAsync(filters);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }
    }
}
