using System.ComponentModel.DataAnnotations;

namespace ShoppingMVC.Web.ViewModels.ApplicationUser
{
    public class RegisterVm
    {
        [Required]

        public string FirstName { get; set; }
        [Required]

        public string LastName { get; set; }
        [Required]

        public string Addres { get; set; }
        [Required]

        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
