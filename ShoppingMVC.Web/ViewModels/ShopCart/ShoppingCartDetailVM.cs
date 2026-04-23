namespace ShoppingMVC.Web.ViewModels.ShopCart
{
    public class ShoppingCartDetailVM
    {
        public int ShoppingCartId { get; set; }
        public int ShoeId { get; set; }
        public int Quantity { get; set; }
        public string ApplicationUserId { get; set; } = null!;
        public Entidades.Shoe ShoeDetail { get; set; } = null!;
    }
}
