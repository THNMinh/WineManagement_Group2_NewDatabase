using BusinessObjects.Entities;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class WareHouseService :IWareHouseService
    {
        private IWareHouseRepository WareHouseRepository;

        public WareHouseService()
        {
            WareHouseRepository = new WareHouseDAO(); // Khởi tạo trực tiếp DAO
        }

        public WareHouse GetWareHouseById(int id)
        {
            return WareHouseRepository.GetWareHouseById(id);
        }

        public IEnumerable<WareHouse> GetAllWareHouses()
        {
            return WareHouseRepository.GetAllWareHouses();
        }

        public void CreateWareHouse(WareHouse WareHouse)
        {
            WareHouseRepository.AddWareHouse(WareHouse);
        }

        public void UpdateWareHouse(WareHouse WareHouse)
        {
            WareHouseRepository.UpdateWareHouse(WareHouse);
        }

        public void DeleteWareHouse(int id)
        {
            WareHouseRepository.DeleteWareHouse(id);
        }

        public IEnumerable<WareHouse> GetWareHousesByAddress(string Address)
        {
            return WareHouseRepository.GetWareHousesByAddress(Address);
        }
    }
}

