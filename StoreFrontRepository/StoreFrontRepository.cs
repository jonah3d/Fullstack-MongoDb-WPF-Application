using MongoDB.Bson;
using MongoDB.Driver;
using StoreFrontDb;
using StoreFrontModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreFrontRepository
{
    public class StoreFrontRepository : IStoreFront
    {
        MongoContext context = null;
        IMongoDatabase mongoDatabase = null;

        public StoreFrontRepository()
        {
            try
            {
                context = new MongoContext();
                mongoDatabase = context.mongoDatabase;

            }
            catch (StoreFrontException ex)
            {
               throw new StoreFrontException($"Error Connecting to Database {ex.Message}");

            }
        }


        public bool CreateUser(User user)
        {
            try
            {
                mongoDatabase.GetCollection<User>("users").InsertOne(user);
                return true;
            }catch(Exception ex)
            {
                throw new StoreFrontException($"Error Creating New User { ex.Message }");
            }
        }
        public List<Product> GetAllMenProduct()
        {
            try
            {
                
                var menCat = mongoDatabase.GetCollection<Category>("category")
                    .Find(x => x.Name == "Men")
                    .FirstOrDefault();

                if (menCat == null)
                {
                   
                    return new List<Product>();
                }

                // Use MongoDB's AnyEq filter builder for array elements
                var filter = Builders<Product>.Filter.AnyEq(p => p.CategoryIds, menCat.Id);

                var menProducts = mongoDatabase.GetCollection<Product>("products")
                    .Find(filter)
                    .ToList();

                if (menProducts == null || menProducts.Count == 0)
                {
                    return new List<Product>();
                }

                return menProducts;
            }
            catch (Exception ex)
            {
         
                throw new StoreFrontException($"Error Getting All Men Shoes: {ex.Message}");
            }
        }

        public Product GetProductByName(string name)
        {
            Product product = null;
            if (name == null)
            {
                throw new StoreFrontException("Product Name cannot be null");
            }

            try
            {
           
                product = mongoDatabase.GetCollection<Product>("products")
                    .Find(x => x.Name == name).FirstOrDefault();

                if (product == null)
                {
                    throw new StoreFrontException("Product not found");
                }

          
                var categoriesCollection = mongoDatabase.GetCollection<Category>("category");
                product.Categories = new List<Category>();

                foreach (var categoryId in product.CategoryIds)
                {
                    var category = categoriesCollection.Find(c => c.Id == categoryId).FirstOrDefault();
                    if (category != null)
                    {
                        product.Categories.Add(category);
                    }
                }

              
                if (product.IvaTypeId != ObjectId.Empty)
                {
                    product.IvaType = mongoDatabase.GetCollection<Vat>("vat")
                        .Find(v => v.Id == product.IvaTypeId).FirstOrDefault();
                }

           
                if (product.TagId != ObjectId.Empty)
                {
                    product.Tag = mongoDatabase.GetCollection<ProductTag>("tags")
                        .Find(t => t.Id == product.TagId).FirstOrDefault();
                }

                return product;
            }
            catch (Exception ex)
            {
                throw new StoreFrontException($"Error Getting Product by Name {ex.Message}");
            }
        }

        public User LoginUser(string username, string password)
        {

            if(username == null || password == null)
            {
                throw new StoreFrontException("Username or Password cannot be null");
            }

          var user =  mongoDatabase.GetCollection<User>("users").Find
                (x => x.Username == username && x.Password == password).FirstOrDefault();

            if (user == null)
            {
                return null;
            }

            return user;

        }
    }
}
