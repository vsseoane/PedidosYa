using PedidosYa.Models;
using PedidosYa.Repository;
using System;

namespace PedidosYa.BusinessLogic
{
    public class CategoryLogic
    {

        public void CreateCategory(Category aCategory)
        {
            CategoryRepository categoryRepository = new CategoryRepository();
            try
            {
                categoryRepository.SaveInDataBase(aCategory);
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }

    }
}
