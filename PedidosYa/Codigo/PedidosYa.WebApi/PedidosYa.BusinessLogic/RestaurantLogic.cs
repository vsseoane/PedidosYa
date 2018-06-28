using Newtonsoft.Json;
using PedidosYa.DTO;
using PedidosYa.Exceptions;
using PedidosYa.Models;
using PedidosYa.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace PedidosYa.BusinessLogic
{
    public class RestaurantLogic : IRestaurantLogic
    {
        RestaurantRepository restaurantRepository = new RestaurantRepository();
        CategoryRepository categoryRepository = new CategoryRepository();
        public void CreateRestaurant(Restaurant aRestaurant)
        {
            try
            {                
                restaurantRepository.SaveInDataBase(aRestaurant);       
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }               
        }

        public IEnumerable<Restaurant> GetAllRestaurants()
        {
            try
            {
                bool active = true;
                IEnumerable<Restaurant>  restaurants = restaurantRepository.GetWithFilter(x => x.IsActive == active, null, "");
                return restaurants;
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }

        public bool CategoriesAreCorrect(List<Category> categories)
        {
            bool categoriesAreCorrect = true;
            CategoryRepository categoryRepository = new CategoryRepository();
            for (int i = 0; categories != null && categoriesAreCorrect && i< categories.Count; i++ ) {
                Category categoryReceived = categoryRepository.GetCategoryById(categories[i].ID);
                if (categoryReceived == null) {
                    categoriesAreCorrect = false;
                }
            }
            return categoriesAreCorrect;
        }

        public IEnumerable<Restaurant> GetAllRestaurantsWithSimilarNameTo(string nameSimilarRestaurant)
        {
            IEnumerable<Restaurant> restaurantsWithSimilarName = restaurantRepository.FindAllRestaurantsBySimilarNameRestaurant(nameSimilarRestaurant);
            return restaurantsWithSimilarName;
        }

        public IEnumerable<Restaurant> GetAllRestaurantsByCategoryId(int idCategory)
        {
            IEnumerable<Restaurant> restaurantsWithThisCategory = restaurantRepository.FindAllRestaurantsByCategory(idCategory);
            return restaurantsWithThisCategory;
        }

        public Restaurant GetRestaurantById(int idRestaurant)
        {
            bool isActived = true;
            Restaurant restaurantAux = restaurantRepository.GetWithFilter(x => x.ID == idRestaurant && x.IsActive == isActived, null, "").FirstOrDefault<Restaurant>();
            return restaurantAux;
        }
        public Restaurant GetRestaurantByIdWithCategories(int idRestaurant)
        {
            bool isActived = true;
            Restaurant restaurantAux = restaurantRepository.GetWithFilter(x => x.ID == idRestaurant && x.IsActive == isActived, null, "Categories").FirstOrDefault<Restaurant>();
            return restaurantAux;
        }

        public bool IsRestaurantNameRepeat(string restaurantName)
        {
            bool isRepeatRestaurantName = restaurantRepository.ExistsRestaurantName(restaurantName);
            return isRepeatRestaurantName;
        }
        public bool IsRestaurantNameRepeat(string restaurantName, int idRestaurant)
        {
            bool isRepeatRestaurantName = restaurantRepository.ExistsRestaurantName(restaurantName, idRestaurant);
            return isRepeatRestaurantName;
        }
        public List<RestaurantCompetitionFromApi> GetCompetitors(int idRestaurant)
        {
            Restaurant restaurant = this.GetRestaurantByIdWithCategories(idRestaurant);
            if (restaurant == null) {
                throw new RestaurantNotFoundException("No se encuentra ese Restaurante");
            }
            string token = GetTokenFromAPI();
            List<RestaurantCompetitionFromApi> restaurantFromApis = GetAllRestaurantsFromApiWith(idRestaurant,restaurant.Latitude, restaurant.Longitude, restaurant.Categories, token);
            return restaurantFromApis;
        }

        private List<RestaurantCompetitionFromApi> GetAllRestaurantsFromApiWith(int idRestaurant,string latitude, string longitude, List<Category> categories,string token)
        {
           
            List<RestaurantCompetitionFromApi> restaurantsFromApis = CallApiToGetRestaurantsFromApi(latitude, longitude,token);
            int maxCompetitors = Int32.Parse(ConfigurationManager.AppSettings["maxResultsFromCompetitors"].ToString()); 
            Restaurant restaurant = this.GetRestaurantByIdWithCategories(idRestaurant);
            List<RestaurantCompetitionFromApi> restaurantsFromApisToReturn = new List<RestaurantCompetitionFromApi>();
            bool finish = false;
            for (int i= 0; !finish && restaurantsFromApis!= null && i < restaurantsFromApis.Count; i++) {
                RestaurantCompetitionFromApi restaurantCompetitor = restaurantsFromApis[i];
                if (HasAnyCategories(restaurant.Categories, restaurantCompetitor.categories)) {
                    restaurantsFromApisToReturn.Add(restaurantCompetitor);
                }
                if (restaurantsFromApisToReturn.Count == maxCompetitors)
                {
                    finish = true;
                }           
               
            }
            return restaurantsFromApisToReturn;
        }

        private bool HasAnyCategories(List<Category> categoriesFromRestaurant, List<CategoryFromApi> categoriesFromCompetitor)
        {
            bool found = false;
            for (int i = 0; !found && categoriesFromRestaurant !=null && i < categoriesFromRestaurant.Count; i++) {
                CategoryFromApi catAux = categoriesFromCompetitor.Where(c => c.name == categoriesFromRestaurant[i].Name).FirstOrDefault<CategoryFromApi>();
                if (catAux != null) {
                    found = true;
                }                
            }
            return found;
            
        }

        private List<RestaurantCompetitionFromApi> CallApiToGetRestaurantsFromApi(string latitude, string longitude,string token)
        {
            string country = ConfigurationManager.AppSettings["country"].ToString();
            string URL = "http://stg-api.pedidosya.com/public/v1/search/restaurants";
            string urlParameters = "?country="+ country + "&point=" + latitude + "," + longitude ;

            List<RestaurantCompetitionFromApi> restaurantsFromApi = new List<RestaurantCompetitionFromApi>();
            RequestCompetitor requestCompetitor = new RequestCompetitor();

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Authorization", token);

            HttpResponseMessage response = client.GetAsync(urlParameters).Result;
            if (response.IsSuccessStatusCode)
            {
                requestCompetitor = JsonConvert.DeserializeObject<RequestCompetitor>(response.Content.ReadAsStringAsync().Result);               
            }
            restaurantsFromApi = requestCompetitor.data;
            return restaurantsFromApi;

        }

        private string GetTokenFromAPI()
        {          
            string URL = "http://stg-api.pedidosya.com/public/v1/tokens";
            string user = ConfigurationManager.AppSettings["userToGetToken"].ToString(); 
            string password = ConfigurationManager.AppSettings["passwordToGetToken"].ToString(); 
            string urlParameters = "?clientId=" + user +"&clientSecret=" + password ;
            string token = CallApiToGetToken(URL, urlParameters);
            return token;


        }
        public string CallApiToGetToken(string URL, string urlParameters) {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);
            string token = "";
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;
            if (response.IsSuccessStatusCode)
            {
                AccessToken accessToken = JsonConvert.DeserializeObject<AccessToken>(response.Content.ReadAsStringAsync().Result);
                token = accessToken.access_token;
            }
            return token;
        }
      

        public void UpdateRestaurant(int idRestaurant, Restaurant restaurantModified) {

            Restaurant restaurantToDelete = this.GetRestaurantById(idRestaurant);
            if (restaurantToDelete == null)
            {
                throw new RestaurantNotFoundException("Restaurant con id '" + idRestaurant + "' no encontrado.");
            }
            restaurantRepository.ModifyInDataBase(idRestaurant, restaurantModified);
        }

        public void DeleteRestaurant(int id)
        {            
            Restaurant restaurantToDelete = this.GetRestaurantById(id);
            if (restaurantToDelete == null)
            {
                throw new RestaurantNotFoundException("Restaurant con id '" + id + "' no encontrado.");
            }
            restaurantRepository.DeleteRestaurant(restaurantToDelete);   
        }


    }


}
