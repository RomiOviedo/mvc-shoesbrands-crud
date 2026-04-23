using AutoMapper;
using ShoppingMVC.Entidades;
using ShoppingMVC.Web.ViewModels.ApplicationUser;
using ShoppingMVC.Web.ViewModels.Brand;
using ShoppingMVC.Web.ViewModels.Order;
using ShoppingMVC.Web.ViewModels.Shoe;
using ShoppingMVC.Web.ViewModels.ShopCart;

namespace ShoppingMVC.Web.Mappings
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            LoadMappingBrand();
            LoadMappingShoe();
            LoadMappingApplicationUser();
            LoadShoppingCartsMapping();
            LoadOrderHeadersMapping();
        }

        private void LoadOrderHeadersMapping()
        {
            CreateMap<OrderHeaderEditVM, OrderHeader>()
             .ForMember(dest => dest.OrderDetail, opt => opt.MapFrom(
                 src => src.OrderDetail));
        }

        private void LoadShoppingCartsMapping()
        {
            CreateMap<ShoppingCartDetailVM, ShoppingCart>()
                .ForMember(dest => dest.ShoeId, opt => opt.MapFrom(src => src.ShoeId))
                .ForMember(dest => dest.ApplicationUser, opt => opt.Ignore())
                .ForMember(dest => dest.ApplicationUserId, opt => opt.MapFrom(src => src.ApplicationUserId));

            CreateMap<ShoppingCart, OrderDetail>()
                .ForMember(dest=>dest.OrderHeaderId,opt=>opt.Ignore())
                .ForMember(dest=>dest.Shoe,opt=>opt.Ignore())
                .ForMember(dest=>dest.Quantity, opt=>opt.MapFrom(src=>src.Quantity))
                .ForMember(dest=>dest.Price, opt=>opt.MapFrom(src=>src.Quantity==1 ? src.Shoe.Price:src.Shoe.Price*0.9M));
        }

        private void LoadMappingApplicationUser()
        {
            CreateMap<ApplicationUser, ApplicationUserListVM>();
        }

        private void LoadMappingShoe()
        {
            CreateMap<Shoe, ShoeEditVM>().ReverseMap();

            CreateMap<Shoe, ShoeListVM>().ForMember(dest=> dest.BrandName, opt=>opt.MapFrom(b=>b.Brand!.BrandName)).ReverseMap();

        }

        private void LoadMappingBrand()
        {
            CreateMap<Brand, BrandEditVM>().ReverseMap();
            CreateMap<Brand, BrandListVM>();

        }
    }
}
