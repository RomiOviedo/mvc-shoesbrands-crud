using X.PagedList;

namespace ShoppingMVC.Web.ViewModels.Brand
{
    public class BrandIndexVM
    {
        public IPagedList<BrandListVM> Brands { get; set; }
        public string Search { get; set; }
        public bool ShowBrandFilter { get; set; } = false;
    }
}
