using Bogus.DataSets;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppTinhVanCat_aspnetcore.Models.Product
{
    public enum StateOrder // trạng thái đơn hàng
    {
        Sucess,         //hoàn thành
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

        public AppUser Customer { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Ngày hoàn thành")]
        public DateTime Finished { get; set; }

        [Display(Name = "Địa chỉ nhận hàng")]
        public string Adress { get; set; }

        [Display(Name = "Tổng thanh toán")]

        public StateOrder State { get; set; }

        [Display(Name = "Tổng thanh toán")]
        [Column(TypeName = "decimal(18,4)")] //chỉ định độ chính xác với 18 chữa số trong đó có 4 chữ số thập phân 
        [Range(0, int.MaxValue, ErrorMessage = "Nhập giá trị từ {1} đến {2} ")]
        public decimal Price { get; set; }

        [Display(Name = "Ghi chú ")]
        public string CustomNote { get; set; }

        

    }
}
