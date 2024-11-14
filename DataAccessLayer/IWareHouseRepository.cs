using BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IWareHouseRepository
    {
        WareHouse GetWareHouseById(int id);
        IEnumerable<WareHouse> GetAllWareHouses();
        void AddWareHouse(WareHouse WareHouse);
        void UpdateWareHouse(WareHouse WareHouse);
        void DeleteWareHouse(int id);
        IEnumerable<WareHouse> GetWareHousesByAddress(string Address);
        IEnumerable<object> GetWareHouseWineDetails();
    }
}
