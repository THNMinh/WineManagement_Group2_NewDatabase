using BusinessObjects;
using BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IAccountRepository
    {
        Account GetAccountById(int id);
        IEnumerable<Account> GetAllAccounts();
        void AddAccount(Account account);
        void UpdateAccount(Account account);
        void DeleteAccount(int id);
        IEnumerable<Account> GetAccountsByRole(string role); // Admin, Manager, Staff
        Account GetAccountMember(string email);
    }
}
