using TinyBank.Repository.Interfaces;
using TinyBank.Repository.Models;
using System.Text;

namespace TinyBank.Repository.Implementations
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly string _filePath;
        private readonly List<Customer> _customers;

        public CustomerRepository(string filePath)
        {
            _filePath = filePath;
            _customers = LoadData();
        }

        public List<Customer> GetCustomers() => _customers;

        public Customer GetSingleCustomer(int id) =>
            _customers.FirstOrDefault(c => c.Id == id);

        public int AddCustomer(Customer newCustomer)
        {
            newCustomer.Id = _customers.Any() ? _customers.Max(c => c.Id) + 1 : 1;
            _customers.Add(newCustomer);
            SaveData();
            return newCustomer.Id;
        }

        public int DeleteCustomer(int id)
        {
            var customer = _customers.FirstOrDefault(c => c.Id == id);
            if (customer == null) return -1;

            _customers.Remove(customer);
            SaveData();
            return customer.Id;
        }

        public int UpdateCustomer(Customer customer)
        {
            var index = _customers.FindIndex(c => c.Id == customer.Id);
            if (index >= 0)
            {
                _customers[index] = customer;
                SaveData();
            }

            return customer.Id;
        }

        #region HELPERS

        private List<Customer> LoadData()
        {
            var customers = new List<Customer>();

            if (!File.Exists(_filePath))
                return customers;

            using FileStream fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read);
            using StreamReader reader = new StreamReader(fs, Encoding.UTF8);

            reader.ReadLine();

            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                try
                {
                    var customer = FromCsv(line);
                    customers.Add(customer);
                }
                catch
                {
                    continue;
                }
            }

            return customers;
        }

        private Customer FromCsv(string customer)
        {
            var separated = customer.Split(',', StringSplitOptions.RemoveEmptyEntries);

            if (separated.Length != 6)
                throw new FormatException("Customer format is invalid");

            return new Customer
            {
                Id = int.Parse(separated[0]),
                Name = separated[1],
                IdentityNumber = separated[2],
                PhoneNumber = separated[3],
                Email = separated[4],
                CustomerType = Enum.Parse<CustomerType>(separated[5])
            };
        }

        private void SaveData()
        {
            using FileStream fs = new FileStream(_filePath, FileMode.Create, FileAccess.Write);
            using StreamWriter writer = new StreamWriter(fs, Encoding.UTF8);

            writer.WriteLine("Id,Name,IdentityNumber,PhoneNumber,Email,CustomerType");

            foreach (var customer in _customers)
            {
                writer.WriteLine(ToCsv(customer));
            }
        }

        private string ToCsv(Customer customer) =>
            $"{customer.Id},{customer.Name},{customer.IdentityNumber},{customer.PhoneNumber},{customer.Email},{customer.CustomerType}";

        #endregion
    }
}