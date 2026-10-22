using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShoppingMVC.Web.ViewModels.Shoe
{
    public class ShoeListVM
    {
        public int ShoeId { get; set; }

        [DisplayName("Shoe")]
        public string ShoeName { get; set; }
        public decimal Price { get; set; }

        [DisplayName("Brand")]
        public string BrandName { get; set; }
        [DisplayName("Image")]
        public string? ImageUrl { get; set; }



    }
}
