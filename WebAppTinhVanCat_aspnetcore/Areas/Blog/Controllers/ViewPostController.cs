using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAppTinhVanCat_aspnetcore.Models;

namespace WebAppTinhVanCat_aspnetcore.Areas.Blog.Controllers
{
    [Area("Blog")]
    public class ViewPostController : Controller
    {
        private readonly ILogger<ViewPostController> _logger;
        private readonly AppDbContext _context;

        public ViewPostController(ILogger<ViewPostController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        //post
        //post/{categoryslug?}
        [Route("/post/{categoryslug?}")]
        public IActionResult Index(string categoryslug, int page)
        {
            return Content(categoryslug);
        }

        [Route("/post/{postslug}.html")]
        public IActionResult Detail(string postslug) 
        { 
            return Content(postslug);
        }
    }
}
