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
        public async Task<List<Product>> GetAllMenProduct()
        {
            try
            {

                var menCats = await mongoDatabase.GetCollection<Category>("category")
                    .FindAsync(x => x.Name == "Men");

                var menCat = menCats.FirstOrDefault();


                if (menCat == null)
                {
                   
                    return new List<Product>();
                }

                
                var filter = Builders<Product>.Filter.AnyEq(p => p.CategoryIds, menCat.Id);

                var menProducts = await mongoDatabase.GetCollection<Product>("products")
                    .FindAsync(filter);



                var menProductsList = await menProducts.ToListAsync();
                   

                if (menProductsList == null || menProductsList.Count == 0)
                {
                    return new List<Product>();
                }
                else
                {
                    foreach (var product in menProductsList)
                    {
                        var categoriesCollection = mongoDatabase.GetCollection<Category>("category");
                        product.Categories = new List<Category>();
                        foreach (var categoryId in product.CategoryIds)
                        {
                            var categoryCursor = await categoriesCollection.FindAsync(c => c.Id == categoryId);
                            var category = await categoryCursor.FirstOrDefaultAsync();
                            if (category != null)
                            {
                                product.Categories.Add(category);
                            }
                        }
                        if (product.IvaTypeId != ObjectId.Empty)
                        {
                            var vatCursor = await mongoDatabase.GetCollection<Vat>("vat").FindAsync(v => v.Id == product.IvaTypeId);
                            product.IvaType = await vatCursor.FirstOrDefaultAsync();
                        }
                        if (product.TagId != ObjectId.Empty)
                        {
                            var tagCursor = await mongoDatabase.GetCollection<ProductTag>("tag").FindAsync(t => t.Id == product.TagId);
                            product.Tag = await tagCursor.FirstOrDefaultAsync();
                        }
                    }
                }

                    return menProductsList;
            }
            catch (Exception ex)
            {
         
                throw new StoreFrontException($"Error Getting All Men Shoes: {ex.Message}");
            }
        }

        public async Task<List<Product>> GetAllWomenProduct()
        {
            try
            {

                var womenCats= await mongoDatabase.GetCollection<Category>("category")
                .FindAsync(x => x.Name == "Women");

                var women = womenCats.FirstOrDefault();

                if(women == null)
                {
                    return new List<Product>();
                }

                var filter = Builders<Product>.Filter.AnyEq(p => p.CategoryIds, women.Id);


                var womenProducts = await mongoDatabase.GetCollection<Product>("products")
                    .FindAsync(filter);

              var womenProductList =  await womenProducts.ToListAsync();
                if(womenProductList == null)
                {
                    return new List<Product>();
                }
                else
                {
                    foreach (var product in womenProductList)
                    {
                        var categoriesCollection = mongoDatabase.GetCollection<Category>("category");
                        product.Categories = new List<Category>();
                        foreach (var categoryId in product.CategoryIds)
                        {
                            var categoryCursor = await categoriesCollection.FindAsync(c => c.Id == categoryId);
                            var category = await categoryCursor.FirstOrDefaultAsync();
                            if (category != null)
                            {
                                product.Categories.Add(category);
                            }
                        }
                        if (product.IvaTypeId != ObjectId.Empty)
                        {
                            var vatCursor = await mongoDatabase.GetCollection<Vat>("vat").FindAsync(v => v.Id == product.IvaTypeId);
                            product.IvaType = await vatCursor.FirstOrDefaultAsync();
                        }
                        if (product.TagId != ObjectId.Empty)
                        {
                            var tagCursor = await mongoDatabase.GetCollection<ProductTag>("tag").FindAsync(t => t.Id == product.TagId);
                            product.Tag = await tagCursor.FirstOrDefaultAsync();
                        }
                    }
                }

                return womenProductList;
            }

            catch (Exception ex)
            {
                throw new StoreFrontException($"Error Getting All Women Shoes: {ex.Message}");
            }
        }


        public async Task<List<Product>> GetAllSportsProduct()
        {
            try
            {

                var sportsCats = await mongoDatabase.GetCollection<Category>("category")
                .FindAsync(x => x.Name == "Sports");

                var sports = sportsCats.FirstOrDefault();

                if (sports == null)
                {
                    return new List<Product>();
                }

                var filter = Builders<Product>.Filter.AnyEq(p => p.CategoryIds, sports.Id);


                var sportsProducts = await mongoDatabase.GetCollection<Product>("products")
                    .FindAsync(filter);

                var sportsproductList = await sportsProducts.ToListAsync();
                if (sportsproductList == null)
                {
                    return new List<Product>();
                }
                else
                {
                    foreach (var product in sportsproductList)
                    {
                        var categoriesCollection = mongoDatabase.GetCollection<Category>("category");
                        product.Categories = new List<Category>();
                        foreach (var categoryId in product.CategoryIds)
                        {
                            var categoryCursor = await categoriesCollection.FindAsync(c => c.Id == categoryId);
                            var category = await categoryCursor.FirstOrDefaultAsync();
                            if (category != null)
                            {
                                product.Categories.Add(category);
                            }
                        }
                        if (product.IvaTypeId != ObjectId.Empty)
                        {
                            var vatCursor = await mongoDatabase.GetCollection<Vat>("vat").FindAsync(v => v.Id == product.IvaTypeId);
                            product.IvaType = await vatCursor.FirstOrDefaultAsync();
                        }
                        if (product.TagId != ObjectId.Empty)
                        {
                            var tagCursor = await mongoDatabase.GetCollection<ProductTag>("tag").FindAsync(t => t.Id == product.TagId);
                            product.Tag = await tagCursor.FirstOrDefaultAsync();
                        }
                    }
                }

                return sportsproductList;
            }

            catch (Exception ex)
            {
                throw new StoreFrontException($"Error Getting All Sports Shoes: {ex.Message}");
            }
        }

      

        public async Task<Product> GetProductByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new StoreFrontException("Product Name cannot be null or empty");
            }

            try
            {
                var productCursor = await mongoDatabase.GetCollection<Product>("products")
                    .FindAsync(x => x.Name == name);

                var product = await productCursor.FirstOrDefaultAsync(); 

                if (product == null)
                {
                    throw new StoreFrontException("Product not found");
                }

                var categoriesCollection = mongoDatabase.GetCollection<Category>("category");
                product.Categories = new List<Category>();

                foreach (var categoryId in product.CategoryIds)
                {
                    var categoryCursor = await categoriesCollection.FindAsync(c => c.Id == categoryId);
                    var category = await categoryCursor.FirstOrDefaultAsync();
                    if (category != null)
                    {
                        product.Categories.Add(category);
                    }
                }

                if (product.IvaTypeId != ObjectId.Empty)
                {
                    var vatCursor = await mongoDatabase.GetCollection<Vat>("vat").FindAsync(v => v.Id == product.IvaTypeId);
                    product.IvaType = await vatCursor.FirstOrDefaultAsync();
                }

                if (product.TagId != ObjectId.Empty)
                {
                    var tagCursor = await mongoDatabase.GetCollection<ProductTag>("tag").FindAsync(t => t.Id == product.TagId);
                    product.Tag = await tagCursor.FirstOrDefaultAsync();
                }

                return product;
            }
            catch (Exception ex)
            {
                throw new StoreFrontException($"Error Getting Product by Name: {ex.Message}");
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
