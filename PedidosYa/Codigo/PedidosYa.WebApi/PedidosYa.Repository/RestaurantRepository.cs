using PedidosYa.DataAccess;
using PedidosYa.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Linq.SqlClient;

namespace PedidosYa.Repository
{
    public class RestaurantRepository
    {
        public void SaveInDataBase(Restaurant aRestaurant) {
            try
            {
                using (PersistentDbContext context = new PersistentDbContext())
                {
                    List<Category> categories = aRestaurant.Categories;
                    foreach (var category in categories)
                    {
                        context.Categories.Attach(category);
                    }
                    aRestaurant.IsActive = true;
                    context.Set<Restaurant>().Add(aRestaurant);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }


        public bool ExistsRestaurantName(string restaurantName)
        {
            bool existRestaurantName = false;
            try
            {
                using (PersistentDbContext context = new PersistentDbContext())
                {
                    Restaurant restaurantAux =  this.GetWithFilter(x => x.Name == restaurantName, null, "")
                    .FirstOrDefault<Restaurant>();
                    if (restaurantAux != null) {
                        existRestaurantName = true;
                    }

                    return existRestaurantName;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }

        public void ModifyInDataBase(int idRestaurant, Restaurant restaurantModified)
        {
            try
            {
                using (PersistentDbContext context = new PersistentDbContext())
                {
                    restaurantModified.ID = idRestaurant;
                    restaurantModified.IsActive = true;
                    context.Set<Restaurant>().Attach(restaurantModified);
                    context.Entry(restaurantModified).State = EntityState.Modified;                    
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }

        public IEnumerable<Restaurant> FindAllRestaurantsBySimilarNameRestaurant(string nameSimilarRestaurant)
        {
            using (PersistentDbContext context = new PersistentDbContext())
            {
                IEnumerable<Restaurant> restaurantsAux = this.GetWithFilter(x => x.IsActive == true, null, "");
                IEnumerable<Restaurant> restaurantWithSimilarName = context.Restaurants
                           .Where(p => p.Name.Contains(nameSimilarRestaurant)).ToList<Restaurant>();
                return restaurantWithSimilarName;
            }
          
        }

        public IEnumerable<Restaurant> FindAllRestaurantsByCategory(int categoryId)
        {
            using (PersistentDbContext context = new PersistentDbContext())
            {
                CategoryRepository categoryRepository = new CategoryRepository();
                bool actived = true;
                IEnumerable<Restaurant> restaurantsAux = this.GetWithFilter(x => x.IsActive == actived, null, "Categories");
                IEnumerable<Restaurant> restaurantsWithThisCatgory = restaurantsAux.Where(c => c.Categories.Any(b => b.ID == categoryId)).ToList<Restaurant>();
                return restaurantsWithThisCatgory;
            }
        }

        public bool ExistsRestaurantName(string restaurantName, int idRestaurant)
        {
            bool existRestaurantName = false;
            try
            {
                using (PersistentDbContext context = new PersistentDbContext())
                {
                    Restaurant restaurantAux = this.GetWithFilter(x => x.Name == restaurantName && x.ID != idRestaurant, null, "")
                    .FirstOrDefault<Restaurant>();
                    if (restaurantAux != null)
                    {
                        existRestaurantName = true;
                    }

                    return existRestaurantName;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }

        public IEnumerable<Restaurant> GetWithFilter(
                                        Expression<Func<Restaurant, bool>> filter = null,
                                        Func<IQueryable<Restaurant>, IOrderedQueryable<Restaurant>> orderBy = null,
                                        string includeProperties = "")
        {
            using (PersistentDbContext context = new PersistentDbContext())
            {
                IQueryable<Restaurant> query = context.Set<Restaurant>();

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }

                if (orderBy != null)
                {
                    return orderBy(query).ToList();
                }
                else
                {
                    return query.ToList();
                }
            }
        }

        public void DeleteRestaurant(Restaurant restaurantToDelete)
        {
            restaurantToDelete.IsActive = false;
            this.ModifyInDataBase(restaurantToDelete.ID, restaurantToDelete);
        }
    }
}
