using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebAppTinhVanCat_aspnetcore.Models;
using WebAppTinhVanCat_aspnetcore.Models.Product;

namespace WebAppTinhVanCat_aspnetcore.Areas.Product.Controllers
{
    [Area("Product")]
    [Route("/product/trademark/{action}/")]
    public class TradeMarkController : Controller
    {
        private readonly AppDbContext _context;

        public TradeMarkController(AppDbContext context)
        {
            _context = context;
        }


        // GET: TradeMarkController
        public ActionResult Index()
        {
            
            return View( _context.TradeMarks.ToList());
        }


        // GET: TradeMarkController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TradeMarkController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TradeMarkModel trademark)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.TradeMarks.Add(trademark);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }
            return View();

        }

        // GET: TradeMarkController/Edit/5
        public ActionResult Edit(int id)
        {
            var trade = _context.TradeMarks.Find(id);
            if(trade == null ) return NotFound("error! không tìm thấy thương hiệu!");
            return View(trade);
        }

        // POST: TradeMarkController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TradeMarkController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TradeMarkController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
