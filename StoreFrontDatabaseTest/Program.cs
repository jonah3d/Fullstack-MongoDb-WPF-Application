using StoreFrontRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreFrontDatabaseTest
{
    public  class Program
    {

        static void loginClient()
        {
            IStoreFront storeFront = new StoreFrontRepository.StoreFrontRepository();
            var ans = storeFront.LoginUser("jonah3d", "jonah3darthur");

            if(ans)
            {
                Console.WriteLine("Login Successful");
            }
            else
            {
                Console.WriteLine("Login Failed");
            }
        }

        public static void  Main()
        {
            Program.loginClient();  
        }

    }
}
