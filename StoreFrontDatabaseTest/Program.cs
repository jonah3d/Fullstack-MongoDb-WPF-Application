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

            if(ans!=null)
            {
                Console.WriteLine("Login Successful");
            }
            else
            {
                Console.WriteLine("Login Failed");
            }
        }

        static void GetProduct()
        {
            IStoreFront storeFront = new StoreFrontRepository.StoreFrontRepository();
            var product = storeFront.GetProductByName("Air Jordan 1 Low Premium");
            Console.WriteLine("GetProduct() executed");

            if (product != null)
            {
                Console.WriteLine($"\nID: {product.Id}");
                Console.WriteLine($"\nNAME: {product.Name}");
                Console.WriteLine($"\nDESCRIPTION: {product.Description}");

         
                if (product.Categories != null)
                {
                    foreach (var category in product.Categories)
                    {
                        Console.WriteLine($"\nCATEGORY: {category.Name}");
                    }
                }
                else
                {
                    Console.WriteLine("\nNo categories found");
                }

                if (product.IvaType != null)
                {
                    Console.WriteLine($"\nVAT: {product.IvaType.Type}");
                }

              
                if (product.Tag != null)
                {
                    Console.WriteLine($"\nTAG: {product.Tag.Name}");
                }

               
                if (product.Variants != null && product.Variants.Count > 0)
                {
                    Console.WriteLine("\nVARIANTS:");
                    foreach (var variant in product.Variants)
                    {
                        Console.WriteLine($"  Color: {variant.Color}, Price: {variant.Price}");


                        if (variant.Discount != null)
                        {
                            Console.WriteLine($"    Discount: {variant.Discount.Percentage}%");
                           
                        }

                        if (variant.Sizes != null && variant.Sizes.Count > 0)
                        {
                            Console.WriteLine("  Sizes:");
                            foreach (var size in variant.Sizes)
                            {
                                Console.WriteLine($"    {size.Size}: {size.Stock} in stock");
                            }
                        }

                     
                        if (variant.Photos != null && variant.Photos.Count > 0)
                        {
                            Console.WriteLine("  Photos:");
                            foreach (var photo in variant.Photos)
                            {
                                Console.WriteLine($"    {photo.Url}");
                            }
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Product not found");
            }
        }


        static void GetAllMenProduct()
        {
            IStoreFront storeFront = new StoreFrontRepository.StoreFrontRepository();
            var menproducts = storeFront.GetAllMenProduct();


            if (menproducts != null && menproducts.Any())
            {
                Console.WriteLine("Men products found: " + menproducts.Count);
                for (int i = 0; i < menproducts.Count;i++)
                {
                    var product = menproducts[i];
                    Console.WriteLine($"{i}. {product.Name}");
                }
            }
            else
            {
                Console.WriteLine("No Products Found or list is empty");
            }
        }


        public static void  Main()
        {
            Program.loginClient();  
            Console.WriteLine("===================================\n");
        Program.GetProduct();

          // Program.GetAllMenProduct();

        }

    }
}
