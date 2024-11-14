using BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    interface IWarehouseWineService
    {
        WarehouseWine GetWarehouseWineById(int id);
        IEnumerable<WarehouseWine> GetAllWarehouseWines();
        void CreateWarehouseWine(WarehouseWine WareHouse);
        void UpdateWarehouseWine(WarehouseWine WareHouse);
        void DeleteWareHouse(int id);
    }
}
