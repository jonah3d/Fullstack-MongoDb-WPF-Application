using StoreFrontModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreFrontRepository
{
    public interface IStoreFront
    {

        public bool CreateUser(User user);
        public User LoginUser(string username, string password);
        // public User GetUser(string username);

        public Task<List<Product>> GetAllMenProduct();
        public Task<List<Product>> GetAllWomenProduct();
        public Task<List<Product>> GetAllSportsProduct();

        public Task <Product> GetProductByName(string name);
    }
}
