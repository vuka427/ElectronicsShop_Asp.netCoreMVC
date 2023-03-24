using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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


        [TempData]
        public string StatusMessage { get; set; }

        // GET: TradeMarkController
        public ActionResult Index()
        {
            
            return View( _context.TradeMarks.Include(t=>t.Products).ToList());
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
                    StatusMessage = "Thêm thành công !";
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
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var trade = _context.TradeMarks.Find(id);
            if(trade == null ) return NotFound("error! không tìm thấy thương hiệu!");
            return View(trade);
        }

        // POST: TradeMarkController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, TradeMarkModel trademark)
        {
            if (ModelState.IsValid && id == trademark.Id)
            {                
                try
                {
                    StatusMessage = "Cập nhật thành công !";
                    _context.TradeMarks.Update(trademark);
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

        // GET: TradeMarkController/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var trade = _context.TradeMarks.Find(id);
            if (trade == null) return NotFound("error! không tìm thấy thương hiệu!");
            return View(trade);
        }

        // POST: TradeMarkController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            var trademark = _context.TradeMarks.Include(m=>m.Products).Where(t=>t.Id==id).FirstOrDefault();
            if (trademark == null) return NotFound("error! không tìm thấy thương hiệu!");
            if (trademark.Products.Count !=0 )
            {
                StatusMessage = "Error ! không xóa được, tồn tại sản phẩm sử dụng thương hiệu này !";
                return RedirectToAction(nameof(Index));
            }
            try
            {
                StatusMessage = "Xóa thành công !";
                _context.TradeMarks.Remove(trademark);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                StatusMessage = "Error ! không xóa được !";
                return RedirectToAction(nameof(Index));
            }
         
        }
    }
}
