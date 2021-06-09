using Catalog.API.Data.Interfaces;
using Catalog.API.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContext : ICatalogContext
    {
        public IMongoCollection<Product> Products { get; }

        public CatalogContext(IConfiguration config)
        {
            var connectionString = config.GetValue<string>("DatabaseSettings:ConnectionString");
            var databaseName = config.GetValue<string>("DatabaseSettings:DatabaseName");
            var collectionName = config.GetValue<string>("DatabaseSettings:CollectionName");

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            Products = database.GetCollection<Product>(collectionName);
        }
    }
}
