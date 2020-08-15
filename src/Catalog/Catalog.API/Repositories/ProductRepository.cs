using Catalog.API.Data.Interfaces;
using Catalog.API.Entities;
using Catalog.API.Repositories.Interfaces;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _context;

        public ProductRepository(ICatalogContext context) => _context = context;

        public async Task<IEnumerable<Product>> GetProducts() => await _context.Products.FindSync(x => true).ToListAsync();

        public async Task<Product> GetProduct(string id) => await _context.Products.FindSync(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<IEnumerable<Product>> GetProductByName(string name) => await _context.Products.FindSync(x => x.Name == name ).ToListAsync();

        // ესეთი ვარიანტიც შეიძლება ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
        public async Task<IEnumerable<Product>> GetProductByNames(string name)
        {
            var filter = Builders<Product>.Filter.ElemMatch(x => x.Name, name);

            return await _context.Products.FindSync(filter).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string category) => await _context.Products.FindSync(x => x.Category == category).ToListAsync();

        public async Task Create(Product product) => await _context.Products.InsertOneAsync(product);

        public async Task<bool> Update(Product product)
        {
            var updateResult = await _context.Products.ReplaceOneAsync(x => x.Id == product.Id, product);

            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Delete(string id)
        {
            var deleteResult = await _context.Products.DeleteOneAsync(x => x.Id == id);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }
    }
}
