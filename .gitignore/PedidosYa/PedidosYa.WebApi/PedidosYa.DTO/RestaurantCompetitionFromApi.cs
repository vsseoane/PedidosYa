using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PedidosYa.DTO
{
    public class RestaurantCompetitionFromApi
    {
        public string name { get; set; }
        public List<CategoryFromApi> categories { get; set; }
    }
}
