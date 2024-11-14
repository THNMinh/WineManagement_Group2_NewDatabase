using BusinessObjects.Entities;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    internal class WarehouseWineService : IWarehouseWineService
    {
        private IWarehouseWineRepository _repository;
        public WarehouseWineService()
        {
            _repository = new WarehouseWineDAO(); // Khởi tạo trực tiếp DAO
        }
        public void CreateWarehouseWine(WarehouseWine WareHouse)
        {
            _repository.AddWarehouseWine(WareHouse);
        }

        public void DeleteWareHouse(int id)
        {
            _repository.DeleteWarehouseWine(id);
        }

        public IEnumerable<WarehouseWine> GetAllWarehouseWines()
        {
           return _repository.GetAllWarehouseWines();
        }

        public WarehouseWine GetWarehouseWineById(int id)
        {
            return _repository.GetWarehouseWineById(id);
        }

        public void UpdateWarehouseWine(WarehouseWine WareHouse)
        {
            _repository.UpdateWarehouseWine(WareHouse);
        }
    }
}
