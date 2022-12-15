using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppTinhVanCat_aspnetcore.Models
{
    [Table("Post")]
    public class Post
    {
        [Key]
        public int PostId { set; get; }

        [Required(ErrorMessage = "Phải có tiêu đề bài viết")]
        [Display(Name = "Tiêu đề")]
        [StringLength(160, MinimumLength = 5, ErrorMessage = "{0} dài {1} đến {2}")]
        public string Title { set; get; }

        [Display(Name = "Mô tả ngắn")]
        public string Description { set; get; }

        [Display(Name = "Chuỗi định danh (url)", Prompt = "Nhập hoặc để trống tự phát sinh theo Title")]
        [StringLength(160, MinimumLength = 5, ErrorMessage = "{0} dài {1} đến {2}")]
        [RegularExpression(@"^[a-z0-9-]*$", ErrorMessage = "Chỉ dùng các ký tự [a-z0-9-]")]
        public string Slug { set; get; } //chuổi url

        [Display(Name = "Nội dung")]
        public string Content { set; get; }

        [Display(Name = "Xuất bản")]
        public bool Published { set; get; } //có hiển thị ra hay không

        [Required]
        [Display(Name = "Tác giả")]
        public string AuthorId { set; get; }

        [ForeignKey("AuthorId")]
        [Display(Name = "Tác giả")]
        public virtual AppUser Author { set; get; }

        [Display(Name = "Ngày tạo")]
        public DateTime DateCreated { set; get; }

        [Display(Name = "Ngày cập nhật")]
        public DateTime DateUpdated { set; get; }

        public virtual List<PostCategory> PostCategories { get; set; }

    }
}
