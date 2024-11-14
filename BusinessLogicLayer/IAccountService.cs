using BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public interface IAccountService
    {
        Account GetAccountById(int id);
        IEnumerable<Account> GetAllAccounts();
        void CreateAccount(Account account);
        void UpdateAccount(Account account);
        void DeleteAccount(int id);
        IEnumerable<Account> GetAccountsByRole(string role); // Admin, Manager, Staff
    }

}
