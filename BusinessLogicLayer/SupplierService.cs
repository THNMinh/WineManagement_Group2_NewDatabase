using BusinessObjects.Entities;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class SupplierService : ISupplierService
    {
        private ISupplierRepository supplierRepository;

        public SupplierService()
        {
            supplierRepository = new SupplierDAO(); // Khởi tạo trực tiếp DAO
        }

        public Supplier GetSupplierById(int id)
        {
            return supplierRepository.GetSupplierById(id);
        }

        public IEnumerable<Supplier> GetAllSuppliers()
        {
            return supplierRepository.GetAllSuppliers();
        }

        public void CreateSupplier(Supplier supplier)
        {
            supplierRepository.AddSupplier(supplier);
        }

        public void UpdateSupplier(Supplier supplier)
        {
            supplierRepository.UpdateSupplier(supplier);
        }

        public void DeleteSupplier(int id)
        {
            supplierRepository.DeleteSupplier(id);
        }
    }

}
