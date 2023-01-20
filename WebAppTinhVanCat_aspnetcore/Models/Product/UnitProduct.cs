using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppTinhVanCat_aspnetcore.Models.Product
{
    [Table("UnitProduct")]
    public class UnitProduct
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Đơn vị")]
        public string Unit { get; set; }


    }
}
