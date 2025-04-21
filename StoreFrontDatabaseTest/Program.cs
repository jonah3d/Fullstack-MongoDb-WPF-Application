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


        static async Task GetCompanyInfo()
        {
            IStoreFront storeFront = new StoreFrontRepository.StoreFrontRepository();
            var company = await storeFront.GetCompanyInfo();

            if(company != null)
            {
                Console.WriteLine("Company Id: " + company.Id);
                Console.WriteLine("Company Name: " + company.Name);
                Console.WriteLine("Company NIF: " + company.Nif);
                Console.WriteLine("Company Registro Mercantil: " + company.RegistroMercantil);
                Console.WriteLine("Company Phone: " + company.Phone);
                Console.WriteLine("Company Email: " + company.Email);
                foreach (var address in company.Addresses)
                {
                    Console.WriteLine("Company Address: " + address.Street + ", " + address.City + ", " + address.PostalCode + ", " + address.Provincia + ", " + address.Country);
                }
            }
            else
            {
                Console.WriteLine("Company not found");
            }
        }

        static async Task GetProduct()
        {
            IStoreFront storeFront = new StoreFrontRepository.StoreFrontRepository();
            var product = await storeFront.GetProductByName("Nike Air Force 1");
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
                else
                {
                    Console.WriteLine("\nNo tags found");
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


        static async Task GetAllMenProduct()
        {
            IStoreFront storeFront = new StoreFrontRepository.StoreFrontRepository();
            var menproducts = await storeFront.GetAllMenProduct();


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


        public static async Task  Main()
        {
            Program.loginClient();  
            Console.WriteLine("===================================\n");
     //  await Program.GetProduct();

        // await  Program.GetAllMenProduct();

            await GetCompanyInfo();

        }

    }
}
