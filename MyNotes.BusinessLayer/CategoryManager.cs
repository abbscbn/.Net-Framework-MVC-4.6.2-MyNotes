using MyNotes.DataAccessLayer.EntityFramework;
using MyNotes.Entities;
using System.Collections.Generic;

namespace MyNotes.BusinessLayer
{
    public class CategoryManager
    {
        Repository<Category> repo_category = new Repository<Category>();
        public List<Category> GetAllCategorys()
        {
            return repo_category.List();
        }

        public Category GetCategoryById(int id)
        {
            return repo_category.Find(x => x.Id == id);
        }
    }
}
