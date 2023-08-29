using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models
{
    public class SignInModel
    {

        [EmailAddress]
        [Key]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
