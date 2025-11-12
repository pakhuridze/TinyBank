using System.Text.Json;
using TinyBank.Repository.Interfaces;
using TinyBank.Repository.Models;

namespace TinyBank.Repository.Implementations
{
    public class AccountRepository : IAccountRepository
    {
        private readonly string _filePath;
        private readonly List<Account> _accounts;

        public AccountRepository(string filePath)
        {
            _filePath = filePath;
            _accounts = LoadData();
        }

        public List<Account> GetAccounts() => _accounts;

        public Account GetSingleAccount(int id) =>
            _accounts.FirstOrDefault(a => a.Id == id);

        public List<Account> GetAccountsOfCustomer(int customerId) =>
            _accounts.Where(a => a.CustomerId == customerId).ToList();

        public int AddAccount(Account newAccount)
        {
            newAccount.Id = _accounts.Any() ? _accounts.Max(c => c.Id) + 1 : 1;
            _accounts.Add(newAccount);
            SaveData();
            return newAccount.Id;
        }

        public int DeleteAccount(int id)
        {
            var account = _accounts.FirstOrDefault(a => a.Id == id);
            if (account == null) return -1;

            _accounts.Remove(account);
            SaveData();
            return account.Id;
        }

        public int UpdateAccount(Account account)
        {
            var index = _accounts.FindIndex(a => a.Id == account.Id);
            if (index >= 0)
            {
                _accounts[index] = account;
                SaveData();
            }
            return account.Id;
        }

        #region HELPERS

        private List<Account> LoadData()
        {
            if (!File.Exists(_filePath))
                return new List<Account>();

            using FileStream fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read);
            try
            {
                var accounts = JsonSerializer.Deserialize<List<Account>>(fs,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return accounts ?? new List<Account>();
            }
            catch
            {
                return new List<Account>();
            }
        }

        private void SaveData()
        {
            using FileStream fs = new FileStream(_filePath, FileMode.Create, FileAccess.Write);
            using StreamWriter writer = new StreamWriter(fs);

            var json = JsonSerializer.Serialize(_accounts, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            writer.Write(json);
        }

        #endregion
    }
}
