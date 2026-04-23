using ShoppingMVC.Entidades;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShoppingMVC.Web.ViewModels.Order
{
    public class OrderHeaderEditVM
    {
        public int OrderHeaderId { get; set; }
        public string ApplicationUserId { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public DateTime ShippingDate { get; set; }
        public decimal OrderTotal { get; set; }
        public List<OrderDetail> OrderDetail { get; set; } = new List<OrderDetail>();
        public Entidades.ApplicationUser? ApplicationUser { get; set; }

        [Required(ErrorMessage ="{0} is required")]
        [StringLength(100, ErrorMessage ="{0} must have between {2} and {1} characters", MinimumLength =3)]
        [DisplayName("First Name")]
        public string FirstName { get; set; } = null!;


        [Required(ErrorMessage ="{0} is required")]
        [StringLength(100, ErrorMessage ="{0} must have between {2} and {1} characters", MinimumLength =3)]
        [DisplayName("Last Name")]
        public string LastName { get; set; } = null!;


       [Required(ErrorMessage ="{0} is required")]
        [StringLength(200, ErrorMessage ="{0} must have between {2} and {1} characters", MinimumLength =3)]
        [DisplayName("Address")]
        public string Address { get; set; } = null!;


    }
}
