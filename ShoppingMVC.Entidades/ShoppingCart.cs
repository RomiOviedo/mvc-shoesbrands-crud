using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingMVC.Entidades
{
    public class ShoppingCart
    {
        public int ShoppingCartId { get; set; }
        public int Quantity { get; set; }
        public string ApplicationUserId { get; set; } = null!;
        public DateTime LastUpDated { get; set; } = DateTime.Now;
        public int ShoeId { get; set; }
        public Shoe Shoe { get; set; }= null!;
        public ApplicationUser ApplicationUser { get; set; } = null!;
    }
}
