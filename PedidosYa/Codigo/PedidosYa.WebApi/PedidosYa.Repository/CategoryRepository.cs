using PedidosYa.DataAccess;
using PedidosYa.Models;
using System;

namespace PedidosYa.Repository
{
    public class CategoryRepository
    {
        public Category GetCategoryById(int idCategory)
        {
            try
            {
                using (PersistentDbContext context = new PersistentDbContext())
                {
                    Category categoryAux = context.Set<Category>().Find(idCategory);
                    return categoryAux;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }
        public void SaveInDataBase(Category aCategory)
        {
            try
            {
                using (PersistentDbContext context = new PersistentDbContext())
                {
                    context.Set<Category>().Add(aCategory);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }
    }
}
