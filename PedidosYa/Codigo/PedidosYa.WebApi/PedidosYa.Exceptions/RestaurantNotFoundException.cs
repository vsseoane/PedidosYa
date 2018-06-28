using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PedidosYa.Exceptions
{
    public class RestaurantNotFoundException : Exception
    {
        public RestaurantNotFoundException(string message) : base(message)
        { }
    }
}
