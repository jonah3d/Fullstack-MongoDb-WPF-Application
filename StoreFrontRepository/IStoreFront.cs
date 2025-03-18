using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreFrontRepository
{
    public interface IStoreFront
    {

        public bool CreateUser();
        public bool LoginUser(string username, string password);

    }
}
