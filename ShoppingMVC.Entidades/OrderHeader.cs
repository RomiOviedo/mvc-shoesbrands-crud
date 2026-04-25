using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingMVC.Entidades
{
    public class OrderHeader
    {
        public int OrderHeaderId { get; set; }
        public string ApplicationUserId { get; set; } = null!;
        public DateTime  OrderDate { get; set; }
        public DateTime ShippingDate { get; set; }
        public decimal OrderTotal { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public List<OrderDetail> OrderDetail { get; set; } = new List<OrderDetail>();
        //[ValidateNever]
        public ApplicationUser? ApplicationUser { get; set; }

    }
}
