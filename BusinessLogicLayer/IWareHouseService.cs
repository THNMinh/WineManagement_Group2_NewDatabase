using BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public interface IWareHouseService
    {
        WareHouse GetWareHouseById(int id);
        IEnumerable<WareHouse> GetAllWareHouses();
        void CreateWareHouse(WareHouse WareHouse);
        void UpdateWareHouse(WareHouse WareHouse);
        void DeleteWareHouse(int id);
        IEnumerable<WareHouse> GetWareHousesByAddress(string address);

    }
}
