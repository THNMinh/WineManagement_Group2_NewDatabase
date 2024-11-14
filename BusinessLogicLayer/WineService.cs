using BusinessObjects.Entities;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class WineService : IWineService
    {
        private IWineRepository wineRepository;

        public WineService()
        {
            wineRepository = new WineDAO(); // Khởi tạo trực tiếp DAO
        }

        public Wine GetWineById(int id)
        {
            return wineRepository.GetWineById(id);
        }

        public IEnumerable<Wine> GetAllWines()
        {
            return wineRepository.GetAllWines();
        }

        public void CreateWine(Wine wine)
        {
            wineRepository.AddWine(wine);
        }

        public void UpdateWine(Wine wine)
        {
            wineRepository.UpdateWine(wine);
        }

        public void DeleteWine(int id)
        {
            wineRepository.DeleteWine(id);
        }

        public IEnumerable<Wine> GetWinesByCategory(int categoryId)
        {
            return wineRepository.GetWinesByCategory(categoryId);
        }
    }
}
