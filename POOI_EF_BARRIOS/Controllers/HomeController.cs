using System.Web.Mvc;

namespace POOI_EF.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index() { return RedirectToAction("Index", "Consumidor"); }
    }
}
