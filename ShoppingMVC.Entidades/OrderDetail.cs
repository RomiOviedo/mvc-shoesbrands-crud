using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingMVC.Entidades
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderHeaderId { get; set; }
        public int ShoeId { get; set; }
        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public Shoe? Shoe { get; set; }
        public OrderHeader? OrderHeader { get; set; }

    }
}
