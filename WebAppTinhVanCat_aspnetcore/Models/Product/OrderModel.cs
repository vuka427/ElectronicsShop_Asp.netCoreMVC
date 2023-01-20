using Bogus.DataSets;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace WebAppTinhVanCat_aspnetcore.Models.Product
{
    public enum StateOrder // trạng thái đơn hàng
    {
        Sucess ,         //hoàn thành
        Received,       //đã nhận chữa xử lý
        Accept,         //chấp nhận đơn hàng
        Transport,      //đang vận chuyển
        False           //không hoàn thành
    }


    [Table("Order")]
    public class OrderModel
    {
        [Key]
        public int OrderId { get; set; }

        [Display(Name = "ID khách hàng")]
        [AllowNull]
        public string CustomerID { set; get; }

        [ForeignKey("CustomerID")]
        [AllowNull]
        public AppUser Customer { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Ngày hoàn thành")]
        public DateTime Finished { get; set; }

        [Display(Name = "Trạng thái đơn hàng")]
        public StateOrder State { get; set; }

        [Display(Name = "Địa chỉ nhận hàng")]
        public string Address { get; set; }

        [Display(Name = "Họ Tên")]
        public string FullName { get; set; }

        [Display(Name = "Địa chỉ Email")]
        public string Email { get; set; }

        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; }

        [Display(Name = "Mã vận chuyển")]
        public string TransportCode { get; set; }

        [Display(Name = "Tổng thanh toán")]
        [Column(TypeName = "decimal(18,4)")] //chỉ định độ chính xác với 18 chữa số trong đó có 4 chữ số thập phân 
        [Range(0, int.MaxValue, ErrorMessage = "Nhập giá trị từ {1} đến {2} ")]
        public decimal Price { get; set; }

        [Display(Name = "Ghi chú của khách hàng")]
        [StringLength(255, MinimumLength = 0, ErrorMessage = "{0} dài {1} đến {2}")]
        public string CustomNote { get; set; }

        [Display(Name = "Số tài khoản")]
        public string BankAccountNumber { get; set; }
        

        [Display(Name = "Ghi chú của Shop")]
        [StringLength(255, MinimumLength = 0, ErrorMessage = "{0} dài {1} đến {2}")]
        public string ShopNote { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } //nav list sản phẩm 

    }
}
