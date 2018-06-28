using System.Collections.Generic;

namespace PedidosYa.Models
{
    public class Category
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<Restaurant> Restaurants { get; set; }

     
    }
}
