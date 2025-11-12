using System;
using System.IO;
using System.Linq;
using TinyBank.Repository.Implementations;
using TinyBank.Repository.Models;
using Xunit;

namespace TinyBank.Tests
{
    public class CustomerRepositoryTests
    {
        private string GetTempFilePath()
        {
            string path = Path.Combine(Path.GetTempPath(), $"customers_{Guid.NewGuid()}.csv");
            return path;
        }

        [Fact]
        public void AddCustomer_ShouldIncreaseCount()
        {
            string path = GetTempFilePath();
            var repo = new CustomerRepository(path);

            var newCustomer = new Customer
            {
                Name = "Test User",
                IdentityNumber = "11111111111",
                PhoneNumber = "+995555000000",
                Email = "test@example.com",
                CustomerType = CustomerType.Physical
            };

            repo.AddCustomer(newCustomer);

            var customers = repo.GetCustomers();

            Assert.Single(customers);
            Assert.Equal("Test User", customers.First().Name);
        }

        [Fact]
        public void DeleteCustomer_ShouldRemoveCustomer()
        {
            string path = GetTempFilePath();
            var repo = new CustomerRepository(path);

            var c1 = new Customer { Name = "User1", IdentityNumber = "123", PhoneNumber = "555", Email = "a@a.com", CustomerType = CustomerType.Physical };
            repo.AddCustomer(c1);

            repo.DeleteCustomer(1);

            Assert.Empty(repo.GetCustomers());
        }

        [Fact]
        public void GetSingleCustomer_ShouldReturnCorrectCustomer()
        {
            string path = GetTempFilePath();
            var repo = new CustomerRepository(path);

            var c1 = new Customer { Name = "User1", IdentityNumber = "123", PhoneNumber = "555", Email = "a@a.com", CustomerType = CustomerType.Physical };
            repo.AddCustomer(c1);

            var result = repo.GetSingleCustomer(1);

            Assert.NotNull(result);
            Assert.Equal("User1", result.Name);
        }
    }
}
