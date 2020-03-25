using Microsoft.AspNetCore.Mvc;

namespace Workers.Controllers
{
    [Route("/about")]
    public class AboutController : Controller
    {
        [HttpGet]
        public ViewResult Index()
        {
            return View();
        }
    }
}