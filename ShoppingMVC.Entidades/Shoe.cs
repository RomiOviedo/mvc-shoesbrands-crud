namespace ShoppingMVC.Entidades
{
    public class Shoe
    {
        public int ShoeId { get; set; }
        public string ShoeName { get; set; } = null!;

        public int BrandId { get; set; }
        public Brand Brand { get; set; } = null!;
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }    


    }
}
