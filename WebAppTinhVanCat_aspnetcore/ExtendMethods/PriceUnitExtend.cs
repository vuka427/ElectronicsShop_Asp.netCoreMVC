using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebAppTinhVanCat_aspnetcore.ExtendMethods
{
    public static class PriceUnitExtend
    {
        public static string ToPriceUnitVND(this decimal Price)
        {

            string price = string.Format("{0:0,0}", Price);

            return price+"đ";
        }
        public static string ToPriceUnitVND(this int Price)
        {

            string price = string.Format("{0:0,0}", Price);

            return price + "đ";
        }


    } 
}
