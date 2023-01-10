using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppTinhVanCat_aspnetcore.Models.Contacts
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(50)]
        [Required(ErrorMessage = "{0} không được bỏ trống ! ")]
        [Display(Name = "Họ và Tên")]
        public string FullName { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "{0} không được bỏ trống ! ")]
        [EmailAddress(ErrorMessage = "Phải nhập địa chỉ {0} !")]
        public string Email { get; set; }

        [Display(Name = "Ngày gửi")]
        public DateTime DateSent { get; set; }

        [Display(Name = "Nội dung")]
        public string Massage { get; set; }

        [StringLength(10)]
        [Display(Name = "Số điện thoại")]
        [Phone(ErrorMessage = "Phải nhập {0}")]
        public string Phone { get; set; }

    }
}
