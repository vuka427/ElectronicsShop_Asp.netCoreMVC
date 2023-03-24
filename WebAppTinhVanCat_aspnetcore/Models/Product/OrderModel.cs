using Bogus.DataSets;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace WebAppTinhVanCat_aspnetcore.Models.Product
{
    public enum StateOrder // trạng thái đơn hàng
    {
        Sucess ,        //hoàn thành,Đã giao
        Received,       //đã nhận chữa xử lý
        Accept,         //chấp nhận đơn hàng
        Transport,      //đang vận chuyển
        CustomerCancel, //khách hàng hủy đơn
        ShopCancel,     //Shop hủy đơn
        False           //không hoàn thành, có lỗi khác ...
    }


    [Table("Order")]
    public class OrderModel
    {
        public OrderModel()
        {
            OrderCode = Guid.NewGuid().ToString();
        }

        [Key]
        
        public int OrderId { get; set; }

        [Display(Name = "Mã đơn hàng")]
        public string OrderCode { get; set; }

        [Display(Name = "ID khách hàng")]
        [AllowNull]
        public string CustomerID { set; get; }

        [Display(Name = "Ngày đặt hàng")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Ngày hoàn thành")]
        [AllowNull]
        public DateTime Finished { get; set; }

        [Display(Name = "Trạng thái đơn hàng")]
        public StateOrder State { get; set; }

        [Display(Name = "Địa chỉ nhận hàng")]
        public string Address { get; set; }

        [Display(Name = "Họ Tên khách hàng")]
        public string FullName { get; set; }

        [Display(Name = "Địa chỉ Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Số điện thoại")]
        [Phone]
        public string Phone { get; set; }

        [Display(Name = "Mã vận chuyển")]
       
        public string TransportCode { get; set; }

        [Display(Name = "Tổng thanh toán")]
        [Column(TypeName = "decimal(10,0)")] //chỉ định độ chính xác với 10 chữa số trong đó có 0 chữ số thập phân 
        [Range(0, int.MaxValue, ErrorMessage = "Nhập giá trị từ {1} đến {2} ")]
        public decimal Price { get; set; }

        [Display(Name = "Ghi chú của khách hàng")]
        [StringLength(256, MinimumLength = 0, ErrorMessage = "{0} dài {1} đến {2}")]
        [AllowNull]
        public string CustomNote { get; set; }

        [Display(Name = "Lý do hủy đơn của khách hàng")]
        [StringLength(256, MinimumLength = 0, ErrorMessage = "{0} dài {1} đến {2}")]
        [AllowNull]
        public string CustomCancelReason { get; set; }

        [Display(Name = "Số tài khoản")]
        [AllowNull]
        public string BankAccountNumber { get; set; }
        

        [Display(Name = "Ghi chú của Shop")]
        [AllowNull]
        [StringLength(256, MinimumLength = 0, ErrorMessage = "{0} dài {1} đến {2}")]
        public string ShopNote { get; set; }

        [Display(Name = "Lý do hủy đơn của shop")]
        [StringLength(256, MinimumLength = 0, ErrorMessage = "{0} dài {1} đến {2}")]
        [AllowNull]
        public string ShopCancelReason { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } //nav list sản phẩm 


       
    }
}
