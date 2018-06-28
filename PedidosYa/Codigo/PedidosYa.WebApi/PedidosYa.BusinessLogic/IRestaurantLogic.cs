using PedidosYa.DTO;
using PedidosYa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PedidosYa.BusinessLogic
{
    public interface IRestaurantLogic
    {
        IEnumerable<Restaurant> GetAllRestaurants();
        Restaurant GetRestaurantById(int idRestaurant);
        IEnumerable<Restaurant> GetAllRestaurantsWithSimilarNameTo(string nameSimilarRestaurant);
        IEnumerable<Restaurant> GetAllRestaurantsByCategoryId(int idCategory);
        List<RestaurantCompetitionFromApi> GetCompetitors(int idRestaurant);
        void DeleteRestaurant(int id);
        void UpdateRestaurant(int idRestaurant, Restaurant restaurantModified);
        bool IsRestaurantNameRepeat(string restaurantName);
        bool CategoriesAreCorrect(List<Category> categories);
        void CreateRestaurant(Restaurant aRestaurant);
        bool IsRestaurantNameRepeat(string name, int iD);
    }
}
