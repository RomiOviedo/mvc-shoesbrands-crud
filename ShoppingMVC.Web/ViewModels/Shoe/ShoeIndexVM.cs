using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace ShoppingMVC.Web.ViewModels.Shoe
{
    public class ShoeIndexVM
    {
        public IPagedList<ShoeListVM> Shoes  { get; set; }
        public string? Search { get; set; }
        public int? BrandId { get; set; }

        public IEnumerable<SelectListItem> Brands = new List<SelectListItem>();

        public bool ShowBrandFilter { get; set; } = true;
    }
}
