using MyNotes.BusinessLayer.Abstract;
using MyNotes.BusinessLayer.Result;
using MyNotes.Entities;
using MyNotes.Entities.Messages;

namespace MyNotes.BusinessLayer
{
    public class CategoryManager : ManagerBase<Category>
    {
        private BusinessLayerResult<Category> res = new BusinessLayerResult<Category>();

        public new BusinessLayerResult<Category> Update(Category category)
        {

            Category db_category = Find(x => x.Id == category.Id);

            res.Result = db_category;

            if (db_category == null)
            {
                res.AddError(ErrorMessageCode.CategoryNotFound, "Kategori bulunamadı.");
                return res;
            }
            // farklı bir kategori ismi girilmiş mi kontrol et
            if (db_category.Title != category.Title && Find(x => x.Title == category.Title) != null)
            {
                res.AddError(ErrorMessageCode.CategoryAlreadyExists, "Bu isimde başka bir kategori zaten mevcut. Lütfen farklı bir isim deneyiniz.");
                return res;
            }
            res.Result.Title = category.Title;
            res.Result.Description = category.Description;

            int dbResult = base.Update(res.Result);

            if (dbResult < 1)
            {

                res.AddError(ErrorMessageCode.CategoryCouldNotUpdated, "Kategori güncellenemedi.");

            }

            return res;


        }
    }
}
