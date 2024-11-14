using BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IWarehouseWineRepository
    {
        WarehouseWine GetWarehouseWineById(int id);
        IEnumerable<WarehouseWine> GetAllWarehouseWines();
        void AddWarehouseWine(WarehouseWine WareHouse);
        void UpdateWarehouseWine(WarehouseWine WareHouse);
        void DeleteWarehouseWine(int id);
        
    }
}
