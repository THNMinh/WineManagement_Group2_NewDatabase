using BusinessObjects;
using BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class WarehouseWineDAO : IWarehouseWineRepository
    {
        public void AddWarehouseWine(WarehouseWine WarehouseWine)
        {
            using (var db = new WineManagement2Context())
            {
                db.WarehouseWines.Add(WarehouseWine);
                db.SaveChanges();
            }
        }

        public void DeleteWarehouseWine(int id)
        {
            using (var db = new WineManagement2Context())
            {
                var WareHouse = db.WarehouseWines.Find(id);
                if (WareHouse != null)
                {
                    db.WarehouseWines.Remove(WareHouse);
                    db.SaveChanges();
                }
            }
        }

        public IEnumerable<WarehouseWine> GetAllWarehouseWines()
        {
            using (var db = new WineManagement2Context())
            {
                return db.WarehouseWines.ToList();
            }
        }

        public WarehouseWine GetWarehouseWineById(int id)
        {
            using (var db = new WineManagement2Context())
            {
                return db.WarehouseWines.Find(id);
            }
        }

        public void UpdateWarehouseWine(WarehouseWine WareHouse)
        {
            using (var db = new WineManagement2Context())
            {
                db.WarehouseWines.Update(WareHouse);
                db.SaveChanges();
            }
        }
    }
}
