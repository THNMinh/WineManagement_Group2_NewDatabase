using BusinessObjects.Entities;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class CategoryService : ICategoryService
    {
        private ICategoryRepository categoryRepository;

        public CategoryService()
        {
            categoryRepository = new CategoryDAO(); // Khởi tạo trực tiếp DAO
        }

        public Category GetCategoryById(int id)
        {
            return categoryRepository.GetCategoryById(id);
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return categoryRepository.GetAllCategories();
        }

        public void CreateCategory(Category category)
        {
            categoryRepository.AddCategory(category);
        }

        public void UpdateCategory(Category category)
        {
            categoryRepository.UpdateCategory(category);
        }

        public void DeleteCategory(int id)
        {
            categoryRepository.DeleteCategory(id);
        }
    }

}
