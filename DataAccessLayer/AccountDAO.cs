using BusinessObjects;
using BusinessObjects.Entities;


namespace DataAccessLayer
{
    public class AccountDAO : IAccountRepository
    {
        public Account GetAccountById(int id)
        {
            using (var db = new WineManagement2Context())
            {
                return db.Accounts.Find(id);
            }
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            using (var db = new WineManagement2Context())
            {
                return db.Accounts.ToList();
            }
        }

        public void AddAccount(Account account)
        {
            using (var db = new WineManagement2Context())
            {
                db.Accounts.Add(account);
                db.SaveChanges();
            }
        }

        public void UpdateAccount(Account account)
        {
            using (var db = new WineManagement2Context())
            {
                db.Accounts.Update(account);
                db.SaveChanges();
            }
        }

        public void DeleteAccount(int id)
        {
            using (var db = new WineManagement2Context())
            {
                var account = db.Accounts.Find(id);
                if (account != null)
                {
                    db.Accounts.Remove(account);
                    db.SaveChanges();
                }
            }
        }

        public IEnumerable<Account> GetAccountsByRole(string role)
        {
            using (var db = new WineManagement2Context())
            {
                return db.Accounts.Where(a => a.Role == role).ToList();
            }
        }

        public Account GetAccountMember(string email)
        {
            using (var db = new WineManagement2Context())
            {
                return db.Accounts.SingleOrDefault(account => account.Email == email);
            }
        }
    }

}
