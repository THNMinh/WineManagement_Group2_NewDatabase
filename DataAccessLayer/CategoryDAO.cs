using BusinessObjects;
using BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class CategoryDAO : ICategoryRepository
    {
        public Category GetCategoryById(int id)
        {
            using (var db = new WineManagement2Context())
            {
                return db.Categories.Find(id);
            }
        }

        public IEnumerable<Category> GetAllCategories()
        {
            using (var db = new WineManagement2Context())
            {
                return db.Categories.ToList();
            }
        }

        public void AddCategory(Category category)
        {
            using (var db = new WineManagement2Context())
            {
                db.Categories.Add(category);
                db.SaveChanges();
            }
        }

        public void UpdateCategory(Category category)
        {
            using (var db = new WineManagement2Context())
            {
                db.Categories.Update(category);
                db.SaveChanges();
            }
        }

        public void DeleteCategory(int id)
        {
            using (var db = new WineManagement2Context())
            {
                var category = db.Categories.Find(id);
                if (category != null)
                {
                    db.Categories.Remove(category);
                    db.SaveChanges();
                }
            }
        }
    }

}
