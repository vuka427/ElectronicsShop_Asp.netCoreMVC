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
    [Authorize(Roles = RoleName.Administrator)]
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
            DateTime StartDate = new DateTime(datestart.Year, datestart.Month,1,1,0,0);
            DateTime EndDate = new DateTime(dateend.Year, dateend.Month, DateTime.DaysInMonth(dateend.Year, dateend.Month),0,59,59);

            if (StartDate <= EndDate)
            {

                DateTime date = StartDate;
                var Orders = _context.Orders.Where(o => o.State == Models.Product.StateOrder.Accept && o.Finished >= StartDate && o.Finished <= EndDate ).ToList();
                List<string> label = new List<string>();
                List<decimal> data = new List<decimal>();

                while (date <= EndDate)
                {

                   data.Add(Orders.Where(o => o.State == Models.Product.StateOrder.Accept && o.Finished.Month == date.Month && o.Finished.Year == date.Year).Sum(o => o.Price));

                   label.Add(date.Month.ToString()+"/"+date.Year.ToString());

                   date = date.AddMonths(1);   
                }


                return new JsonResult(new { error = 0, datet = StartDate, datee = EndDate, listprice = data.ToArray(), listlabel = label.ToArray() });
            }





            return new JsonResult(new { error = 1, datet = StartDate, datee = EndDate });
        }
    }
}
