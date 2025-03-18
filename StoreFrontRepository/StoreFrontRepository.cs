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


        public bool CreateUser()
        {
            throw new NotImplementedException();
        }

        public bool LoginUser(string username, string password)
        {
            bool ans = false;

            if(username == null || password == null)
            {
                throw new StoreFrontException("Username or Password cannot be null");
            }

          var user =  mongoDatabase.GetCollection<User>("users").Find
                (x => x.Username == username && x.Password == password).FirstOrDefault();

            if (user != null)
            {
                ans = true;
            }


            return ans;
        }
    }
}
