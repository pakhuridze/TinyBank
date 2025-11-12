using TinyBank.Repository.Implementations;
using TinyBank.Repository.Models;

namespace TinyBank.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath = @"../../../Data/customers.csv";

            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

            var customerRepo = new CustomerRepository(filePath);

            Console.WriteLine("Existing customers:");
            foreach (var c in customerRepo.GetCustomers())
            {
                Console.WriteLine($"{c.Id}. {c.Name} - {c.Email} ({c.CustomerType})");
            }

            Console.WriteLine("\nAdding a new customer...");

            var newCustomer = new Customer
            {
                Name = "Nino Giorgadze",
                IdentityNumber = "12345678901",
                PhoneNumber = "+995599123456",
                Email = "nino.giorgadze@example.com",
                CustomerType = CustomerType.Physical
            };

            customerRepo.AddCustomer(newCustomer);

            Console.WriteLine("New customer added!");

            Console.WriteLine("\nUpdated customer list:");
            foreach (var c in customerRepo.GetCustomers())
            {
                Console.WriteLine($"{c.Id}. {c.Name} - {c.Email} ({c.CustomerType})");
            }

            Console.WriteLine("\nSearching customer by ID:");
            var single = customerRepo.GetSingleCustomer(1);
            if (single != null)
                Console.WriteLine($"Found: {single.Name} - {single.Email}");
            else
                Console.WriteLine("Customer not found.");
        }
    }
}