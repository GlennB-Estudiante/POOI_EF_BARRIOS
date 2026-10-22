using System.Linq;
using System.Web.Mvc;
using POOI_EF.Models;

namespace POOI_EF.Controllers
{
    public class ConsumidorController : Controller
    {
        private readonly ConsumidorDAO dao = new ConsumidorDAO();

        private void CargarPaises(string sel = null)
        {
            ViewBag.Paises = new SelectList(dao.Paises().Select(p => new { id = p[0], n = p[1] }), "id", "n", sel);
        }

        public ActionResult Index()
        {
            ViewBag.Mensaje = TempData["Mensaje"];
            return View(dao.Listar());
        }

        public ActionResult Create()
        {
            CargarPaises();
            return View(new Consumidor());
        }

        [HttpPost]
        public ActionResult Create(Consumidor c)
        {
            string msg = dao.Agregar(c);
            if (msg.StartsWith("Error"))
            {
                ViewBag.Mensaje = msg;
                CargarPaises(c.idpais);
                return View(c);
            }
            TempData["Mensaje"] = msg;
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            TempData["Mensaje"] = dao.Eliminar(id);
            return RedirectToAction("Index");
        }

        public ActionResult Update(string id)
        {
            var c = dao.Buscar(id);
            if (c == null) { TempData["Mensaje"] = "Error: el consumidor no existe"; return RedirectToAction("Index"); }
            CargarPaises(c.idpais);
            return View(c);
        }

        [HttpPost]
        public ActionResult Update(Consumidor c)
        {
            string msg = dao.Actualizar(c);
            if (msg.StartsWith("Error"))
            {
                ViewBag.Mensaje = msg;
                CargarPaises(c.idpais);
                return View(c);
            }
            TempData["Mensaje"] = msg;
            return RedirectToAction("Index");
        }

        public ActionResult Details(string id)
        {
            var c = dao.Buscar(id);
            if (c == null) { TempData["Mensaje"] = "Error: el consumidor no existe"; return RedirectToAction("Index"); }
            ViewBag.Mensaje = "Consumidor encontrado correctamente";
            return View(c);
        }
    }
}
