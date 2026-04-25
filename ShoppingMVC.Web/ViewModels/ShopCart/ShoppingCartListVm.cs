using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using ShoppingMVC.Entidades;
using ShoppingMVC.Web.ViewModels.Order;

namespace ShoppingMVC.Web.ViewModels.ShopCart

{
    public class ShoppingCartListVm
    {
        [ValidateNever] //no me valides el modelo con el shopping cart x q desde la vista summary no te mando el shopping cart solo te mando el order header
        public List<ShoppingCart>? ShoppingCarts { get; set; }
        public OrderHeaderEditVM? OrderHeader { get; set; }
    }
}
