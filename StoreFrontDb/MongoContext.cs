using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using MongoDB.Driver;


namespace StoreFrontDb
{
    public class MongoContext
    {
        public IMongoDatabase mongoDatabase { get; set; }

        public MongoContext()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var client = new MongoClient(config.GetConnectionString("storefront"));

            var connection = client.GetDatabase("storefront");
            mongoDatabase = connection;
            if (connection == null)
            {
                throw new StoreFrontException("Connection Failed");
            }
        }
    }
}
