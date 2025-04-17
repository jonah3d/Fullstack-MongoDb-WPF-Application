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

                
                var hashpassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
                user.Password = hashpassword;

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
            if (username == null || password == null)
            {
                throw new StoreFrontException("Username or Password cannot be null");
            }

         
            var user = mongoDatabase.GetCollection<User>("users")
                .Find(x => x.Username == username)
                .FirstOrDefault();

        
            if (user == null || !VerifyPassword(user.Password, password))
            {
                return null;
            }

            return user;
        }

        

        private bool VerifyPassword(string storedPasswordHash, string enteredPassword)
        {
      
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedPasswordHash);
        }

        public async Task<List<Product>> GetAllChildrenProduct()
        {
            try
            {

                var childrenCats = await mongoDatabase.GetCollection<Category>("category")
                .FindAsync(x => x.Name == "Children");
                var children = childrenCats.FirstOrDefault();
                if (children == null)
                {
                    return new List<Product>();
                }
                var filter = Builders<Product>.Filter.AnyEq(p => p.CategoryIds, children.Id);

                var childrenProducts = await mongoDatabase.GetCollection<Product>("products")
                    .FindAsync(filter);
                var childrenProductList = await childrenProducts.ToListAsync();

                if (childrenProductList == null)
                {
                    return new List<Product>();
                }
                else
                {
                    foreach (var product in childrenProductList)
                    {
                        var categoriesCollection = mongoDatabase.GetCollection<Category>("category");
                        product.Categories = new List<Category>();
                        foreach (var categoryId in product.CategoryIds)
                        {           
                            var categoryCursor = categoriesCollection.Find(c => c.Id == categoryId);
                            var category = categoryCursor.FirstOrDefault();
                            if (category != null)
                            {
                                product.Categories.Add(category);
                            }
                        }
                        if (product.IvaTypeId != ObjectId.Empty)
                        {
                            var vatCursor = mongoDatabase.GetCollection<Vat>("vat").Find(v => v.Id == product.IvaTypeId);
                            product.IvaType = vatCursor.FirstOrDefault();
                        }
                        if (product.TagId != ObjectId.Empty)
                        {
                            var tagCursor = mongoDatabase.GetCollection<ProductTag>("tag").Find(t => t.Id == product.TagId);
                            product.Tag = tagCursor.FirstOrDefault();
                        }
                    }
                }

                return childrenProductList;
            }
            catch (Exception ex)
            {
                throw new StoreFrontException($"Error Getting All Children Shoes: {ex.Message}");
            }
        }

        public async Task<List<Product>> GetNewProducts()
        {
            try
            {
             var tag =   await mongoDatabase.GetCollection<ProductTag>("tag")
                    .FindAsync(x => x.Name == "NEW");

              var tagRes = await tag.FirstOrDefaultAsync();
                if (tagRes == null)
                {
                    return new List<Product>();
                }

                var filter = Builders<Product>.Filter.Eq(p => p.TagId, tagRes.Id);

                var products = await mongoDatabase.GetCollection<Product>("products")
                    .FindAsync(filter);
                var productList = await products.ToListAsync();

                if(productList == null)
                {
                    return new List<Product>();
                }
                else
                {
                    foreach (var product in productList)
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
                return productList;
            }
            catch (Exception ex)
            {
                throw new StoreFrontException($"Error Getting New Products: {ex.Message}");
            }
        }

        public async Task<List<Product>> SearchProductsByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return new List<Product>();

            try
            {
                var filter = Builders<Product>.Filter.Regex("Name", new MongoDB.Bson.BsonRegularExpression(name, "i"));
                var products = await mongoDatabase.GetCollection<Product>("products")
                                                  .Find(filter)
                                                  .Limit(3)
                                                  .ToListAsync();
                if(products == null || products.Count == 0)
                {
                    return new List<Product>();
                }
                else
                {
                    foreach (var product in products)
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
                return products;
            }
            catch (Exception ex)
            {
                throw new StoreFrontException($"Error searching products: {ex.Message}");
            }
        }

        public async Task<List<Product>> GetAllProducts()
        {
            try
            {
                var products = await mongoDatabase.GetCollection<Product>("products")
                    .FindAsync(_ => true);
                var productList = await products.ToListAsync();

                if(productList == null || productList.Count<0) 
                {
                    return new List<Product>();
                }
                else
                {
                    foreach (var product in productList)
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
                return productList;

            }
            catch (Exception ex)
            {
                throw new StoreFrontException($"Error Getting All Products: {ex.Message}");
            }
        }

        public async Task<List<Product>> GetReleasedProducts()
        {
            try
            {
                var tag = await mongoDatabase.GetCollection<ProductTag>("tag")
                       .FindAsync(x => x.Name == "RELEASED");

                var tagRes = await tag.FirstOrDefaultAsync();
                if (tagRes == null)
                {
                    return new List<Product>();
                }

                var filter = Builders<Product>.Filter.Eq(p => p.TagId, tagRes.Id);

                var products = await mongoDatabase.GetCollection<Product>("products")
                    .FindAsync(filter);
                var productList = await products.ToListAsync();

                if (productList == null)
                {
                    return new List<Product>();
                }
                else
                {
                    foreach (var product in productList)
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
                return productList;
            }
            catch (Exception ex)
            {
                throw new StoreFrontException($"Error Getting Released Products: {ex.Message}");
            }
        }
    }
}
