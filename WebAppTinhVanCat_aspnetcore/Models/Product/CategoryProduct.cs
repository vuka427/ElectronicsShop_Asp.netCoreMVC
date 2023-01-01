﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppTinhVanCat_aspnetcore.Models.Products
{
    [Table("CategoryProduct")]
    public class CategoryProduct
    {

        [Key]
        public int Id { get; set; }


        // Tiều đề Category
        [Required(ErrorMessage = "Phải có tên danh mục")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} dài {1} đến {2}")]
        [Display(Name = "Tên danh mục")]
        public string Title { get; set; }

        // Nội dung, thông tin chi tiết về Category
        [DataType(DataType.Text)]
        [Display(Name = "Nội dung danh mục")]
        public string Content { set; get; }

        //chuỗi Url
        [Required(ErrorMessage = "Phải tạo url")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} dài {1} đến {2}")]
        [RegularExpression(@"^[a-z0-9-]*$", ErrorMessage = "Chỉ dùng các ký tự [a-z0-9-]")]
        [Display(Name = "Url hiện thị")]
        public string Slug { set; get; }


        // Category cha (FKey)
        [Display(Name = "Danh mục cha")]
        public int? ParentCategoryId { get; set; }

        // Các Category con
        public virtual ICollection<CategoryProduct> CategoryChildren { get; set; }

        [ForeignKey("ParentCategoryId")]
        [Display(Name = "Danh mục cha")]
        public virtual CategoryProduct ParentCategory { set; get; }

        public void ChildCategoryIDs(List<int> lists, ICollection<CategoryProduct> childcates = null) // lấy tất cả id của danh mục con cháu truyên vào list
        {
            if (childcates == null)
            {
                childcates = this.CategoryChildren;

            }
            foreach(CategoryProduct category in childcates)
            {
                lists.Add(category.Id);
                ChildCategoryIDs(lists, category.CategoryChildren);
            }

        }

        public List<CategoryProduct> ListParents() //lấy danh sách danh mục cha hiện có
        {

            List <CategoryProduct> lists = new List<CategoryProduct>();
            var parent = this.ParentCategory;
            while (parent != null)//kiểm tra có tồn tại dang mục cha hay ko nếu có thêm vào danh sách và tiếp tục lập kiểm tra cha của có  danh mục ông nội ko ....
            {
                lists.Add(parent);
                parent = parent.ParentCategory;
            }
            lists.Reverse();//đảo ngược danh sách

            return lists;
        }
        
    }
}
