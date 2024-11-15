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
                // Fetch the account with the given id only if its Status is true
                return db.Accounts.SingleOrDefault(a => a.AccountId == id && a.Status == "true");
            }
        }


        public IEnumerable<Account> GetAllAccounts()
        {
            using (var db = new WineManagement2Context())
            {
                // Only fetch accounts with Status == "true"
                return db.Accounts.Where(a => a.Status == "true").ToList();
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
                // Only fetch accounts with Status == "true"
                return db.Accounts.Where(a => a.Role == role && a.Status == "true").ToList();
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
