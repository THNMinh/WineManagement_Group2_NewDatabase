using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class WineDAO : IWineRepository
    {
        public Wine GetWineById(int id)
        {
            using (var db = new WineManagement2Context())
            {
                return db.Wines.Find(id);
            }
        }

        public IEnumerable<Wine> GetAllWines()
        {
            using (var db = new WineManagement2Context())
            {
                return db.Wines.Include("Supplier").ToList();
            }
        }

        public void AddWine(Wine wine)
        {
            using (var db = new WineManagement2Context())
            {
                db.Wines.Add(wine);
                db.SaveChanges();
            }
        }

        public void UpdateWine(Wine wine)
        {
            using (var db = new WineManagement2Context())
            {
                db.Wines.Update(wine);
                db.SaveChanges();
            }
        }

        public void DeleteWine(int id)
        {
            using (var db = new WineManagement2Context())
            {
                var wine = db.Wines.Find(id);
                if (wine != null)
                {
                    db.Wines.Remove(wine);
                    db.SaveChanges();
                }
            }
        }

        public IEnumerable<Wine> GetWinesByCategory(int categoryId)
        {
            using (var db = new WineManagement2Context())
            {
                return db.Wines.Where(w => w.CategoryId == categoryId).ToList();
            }
        }
    }

}
