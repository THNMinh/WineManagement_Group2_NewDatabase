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
            using (var context = new WineManagement2Context())
            {
                // Kiểm tra Email đã tồn tại chưa
                var existingAccount = context.Accounts.FirstOrDefault(a => a.Email == account.Email);
                if (existingAccount != null)
                {
                    throw new Exception("Email already exists. Please use a different email.");
                }

                // Thêm tài khoản mới
                context.Accounts.Add(account);
                context.SaveChanges();
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
                    account.Status = "false";
                    db.Accounts.Update(account);
                    db.SaveChanges();
                }
            }
        }

        public void DeleteAccount2(int id)
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
