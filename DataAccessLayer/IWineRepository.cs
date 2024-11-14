using BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IWineRepository
    {
        Wine GetWineById(int id);
        IEnumerable<Wine> GetAllWines();
        void AddWine(Wine wine);
        void UpdateWine(Wine wine);
        void DeleteWine(int id);
        IEnumerable<Wine> GetWinesByCategory(int categoryId);
    }
}
