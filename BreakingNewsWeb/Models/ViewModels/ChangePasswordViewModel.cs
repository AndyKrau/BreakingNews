using System.ComponentModel.DataAnnotations;

namespace BreakingNewsWeb.Models.ViewModels
{
    public class ChangePasswordViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }


        [DataType(DataType.Password), Required(ErrorMessage = "Old Password Required")]
        public string OldPassword { get; set; }


        [DataType(DataType.Password), Required(ErrorMessage = "Old Password Required")]
        public string NewPassword { get; set; }
    }
}
