using System;
using System.Web.Mvc;
using POOI_EF.Models;

namespace POOI_EF.Controllers
{
    public class EmpleadoController : Controller
    {
        public ActionResult Index(int? anio)
        {
            int a = anio ?? DateTime.Now.Year;
            ViewBag.Anio = a;
            return View(new ConsumidorDAO().PedidosPorAnio(a));
        }
    }
}
