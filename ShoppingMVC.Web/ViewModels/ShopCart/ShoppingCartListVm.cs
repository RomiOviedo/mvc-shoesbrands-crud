using ShoppingMVC.Entidades;
using ShoppingMVC.Web.ViewModels.Order;

namespace ShoppingMVC.Web.ViewModels.ShopCart

{
    public class ShoppingCartListVm
    {
        public List<ShoppingCart> ShoppingCarts { get; set; }
        public OrderHeaderEditVM? OrderHeader { get; set; }
    }
}
