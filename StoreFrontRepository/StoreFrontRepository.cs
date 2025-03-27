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
                Console.WriteLine("Error: " + ex.Message);

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
