using System.Collections.Generic;

namespace PedidosYa.Models
{
    public class Restaurant
    {
        
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string OpeningHour { get; set; }
        public string ClosingHour { get; set; }
        public string Telephone { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public List<Category> Categories { get; set; }
        public bool IsActive { get; set; }



    }
  
}

