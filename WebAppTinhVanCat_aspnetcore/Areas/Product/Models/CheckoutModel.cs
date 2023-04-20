using System.ComponentModel.DataAnnotations;

namespace WebAppTinhVanCat_aspnetcore.Areas.Product.Models
{
    public class CheckoutModel
    {
        [Display(Name = "Tỉnh thành",Prompt = "Chọn tỉnh thành")]
        [Required(ErrorMessage ="Phải chọn {0} ")]
        public int DcTinh { get; set; }

        [Display(Name = "Quận huyện",Prompt = "Chọn tỉnh quận huyện")]
        [Required(ErrorMessage = "Phải chọn {0} ")]
        public int DcHuyen { get; set; }

        [Display(Name = "Xã phường",Prompt = "Chọn xã phường")]
        [Required(ErrorMessage = "Phải chọn {0} ")]
        public int DcXa { get; set; }

        [Display(Name = "Số nhà/tên đường")]
        [Required(ErrorMessage = "Phải nhập {0} ")]
        public string SoNha { get; set; }

        [Display(Name = "Số điện thoại nhận hàng")]
        [Required(ErrorMessage ="Phải nhập {0}")]
        [RegularExpression(@"^(84|0[1-9])+([0-9]{8})$", ErrorMessage = "Định dạng số điện thoại sai")]
        public string PhoneNumber { get; set; }


        public string Payment { get; set; }


    }
}
