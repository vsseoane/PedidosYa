using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PedidosYa.DTO
{
    public class RequestCompetitor
    {
        public int total { get; set; }
        public List<RestaurantCompetitionFromApi> data { get; set; }
    }
}
