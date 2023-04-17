using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using WebAppTinhVanCat_aspnetcore.Data;
using WebAppTinhVanCat_aspnetcore.Models;

namespace WebAppTinhVanCat_aspnetcore.Areas.Statistic.Controllers
{
    [Area("Statistic")]
    [Route("admin/statistical/[action]/{id?}")]
    [Authorize(Roles = RoleName.Administrator )]
    public class StatisticalController : Controller
    {
        private readonly AppDbContext _context;

        public StatisticalController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
           
            
            return View();
        }

        [HttpPost]
        public IActionResult GetDataApi([FromForm] DateTime datestart, [FromForm] DateTime dateend)
        {


            if(datestart <= dateend)
            { 
                var chartM = _context.Orders.Where(o => o.State == Models.Product.StateOrder.Accept && o.Finished.Month >= datestart.Month && o.Finished.Year >= datestart.Year && o.Finished.Month <= dateend.Month && o.Finished.Year <= dateend.Year).ToList();
                return new JsonResult(new { error = 0, datet = datestart, datee = dateend });
            }

           
                                  


            return new JsonResult( new {error = 1 ,datet = datestart , datee = dateend });
        }
    }
}
