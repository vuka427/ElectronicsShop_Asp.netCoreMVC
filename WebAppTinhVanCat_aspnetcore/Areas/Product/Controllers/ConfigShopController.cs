using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using System.Linq;
using WebAppTinhVanCat_aspnetcore.Data;
using WebAppTinhVanCat_aspnetcore.Models;
using WebAppTinhVanCat_aspnetcore.Models.Product;

namespace WebAppTinhVanCat_aspnetcore.Areas.Product.Controllers
{
    [Area("Product")]
    [Route("config/shop/{action}")]
    [Authorize(Roles = RoleName.Administrator)]
    public class ConfigShopController : Controller
    {
        private readonly AppDbContext _context;

        public ConfigShopController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult UnitProduct()
        {
            return View();
        }



        [HttpGet]
        public IActionResult GetUnitProductApi()
        {
            var unit = _context.UnitProducts.ToList();

            if(unit.Count <= 0 || unit == null)
            {
                return Json(new
                {
                    success = 0,
                    message = "không có đơn vị tính nào !"
                });
            }
             
            return Json(new
            {
                success = 1,
                units = unit
            });
        }

        [HttpPost]  
        public IActionResult CreateUnitProductApi( string unit)
        {
            if (ModelState.IsValid)
            {
                if(unit != null && !_context.UnitProducts.Any(u => u.Unit == unit) )
                {
                    _context.UnitProducts.Add(new UnitProduct() { Unit = unit });
                    _context.SaveChanges();
                }
                 
            }

            return Ok();
        }

        [HttpPost]
        public IActionResult DeleteUnitProductApi(int id)
        {
            if (ModelState.IsValid)
            {
                
                var unit = _context.UnitProducts.Find(id);
                _context.Remove(unit); 
                _context.SaveChanges();
               

            }

            return Ok();
        }


    }
}
