using BusinessObjects;
using BusinessObjects.Entities;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class AccountService : IAccountService
    {
        private IAccountRepository accountRepository;

        public AccountService()
        {
            accountRepository = new AccountDAO(); // Khởi tạo trực tiếp DAO
        }

        public Account GetAccountById(int id)
        {
            return accountRepository.GetAccountById(id);
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            return accountRepository.GetAllAccounts();
        }

        public void CreateAccount(Account account)
        {
            accountRepository.AddAccount(account);
        }

        public void UpdateAccount(Account account)
        {
            accountRepository.UpdateAccount(account);
        }

        public void DeleteAccount(int id)
        {
            accountRepository.DeleteAccount(id);
        }

        public IEnumerable<Account> GetAccountsByRole(string role)
        {
            return accountRepository.GetAccountsByRole(role);
        }
    }

}
