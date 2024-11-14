using BusinessObjects.Entities;
using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class WareHouseDAO : IWareHouseRepository
    {
        public WareHouse GetWareHouseById(int id)
        {
            using (var db = new WineManagement2Context())
            {
                return db.WareHouses.Find(id);
            }
        }

        public IEnumerable<WareHouse> GetAllWareHouses()
        {
            using (var db = new WineManagement2Context())
            {
                return db.WareHouses.Where(w => w.Status == "True").ToList();
            }
        }

        public void AddWareHouse(WareHouse WareHouse)
        {
            using (var db = new WineManagement2Context())
            {
                db.WareHouses.Add(WareHouse);
                db.SaveChanges();
            }
        }

        public void UpdateWareHouse(WareHouse WareHouse)
        {
            using (var db = new WineManagement2Context())
            {
                db.WareHouses.Update(WareHouse);
                db.SaveChanges();
            }
        }

        public void DeleteWareHouse(int id)
        {
            using (var db = new WineManagement2Context())
            {
                var WareHouse = db.WareHouses.Find(id);
                if (WareHouse != null)
                {
                    db.WareHouses.Remove(WareHouse);
                    db.SaveChanges();
                }
            }
        }

        public IEnumerable<WareHouse> GetWareHousesByAddress(string Address)
        {
            using (var db = new WineManagement2Context())
            {
                return db.WareHouses.Where(a => a.Address == Address).ToList();
            }
        }

        public IEnumerable<object> GetWareHouseWineDetails()
        {
            using (var db = new WineManagement2Context())
            {
                var result = from ww in db.WarehouseWines
                             join wh in db.WareHouses on ww.WareHouseId equals wh.WareHouseId
                             join w in db.Wines on ww.WineId equals w.WineId
                             where wh.Status == "True"  // Only active warehouses
                             select new
                             {
                                 WineName = w.Name,
                                 Address = wh.Address,
                                 ContactPerson = wh.ContactPerson,
                                 PhoneNumber = wh.PhoneNumber,
                                 Location = wh.Location,
                                 Quantity = ww.Quantity,
                                 Description = ww.Description
                             };

                return result.ToList();
            }
        }
    }
}
