using BusinessObjects.Entities;
using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class SupplierDAO : ISupplierRepository
    {
        public Supplier GetSupplierById(int id)
        {
            using (var db = new WineManagement2Context())
            {
                return db.Suppliers.Find(id);
            }
        }

        public IEnumerable<Supplier> GetAllSuppliers()
        {
            using (var db = new WineManagement2Context())
            {
                return db.Suppliers.ToList();
            }
        }

        public void AddSupplier(Supplier supplier)
        {
            using (var db = new WineManagement2Context())
            {
                db.Suppliers.Add(supplier);
                db.SaveChanges();
            }
        }

        public void UpdateSupplier(Supplier supplier)
        {
            using (var db = new WineManagement2Context())
            {
                db.Suppliers.Update(supplier);
                db.SaveChanges();
            }
        }

        public void DeleteSupplier(int id)
        {
            using (var db = new WineManagement2Context())
            {
                var supplier = db.Suppliers.Find(id);
                if (supplier != null)
                {
                    db.Suppliers.Remove(supplier);
                    db.SaveChanges();
                }
            }
        }
    }

}
