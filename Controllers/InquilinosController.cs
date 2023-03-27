using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcInmo.Models;
namespace MvcInmo.Controllers
{
    public class InquilinosController : Controller
    {
        private readonly RepositorioInquilino reInq;

        public InquilinosController()
        {
            reInq = new RepositorioInquilino();
        }
        // GET: Inquilinos
        public ActionResult Index()
        {
            var lista = reInq.GetInquilinos();
            return View(lista);
        }

        // GET: Inquilinos/Details/5
        public ActionResult Details(int id)
        {
            var inquilino = reInq.GetInquilino(id);
            return View(inquilino);
        }

        // GET: Inquilinos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Inquilinos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inquilino inquilino)
        {
            try
            {
                // TODO: Add insert logic here
                reInq.Alta(inquilino);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Inquilinos/Edit/5
        public ActionResult Edit(int id)
        {
            var inquilino = reInq.GetInquilino(id);
            return View(inquilino);
        }

        // POST: Inquilinos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inquilino collection)
        {
            Inquilino i = new Inquilino();
            try
            {
                i = reInq.GetInquilino(id);
                i.Nombre = collection.Nombre;
                i.Apellido = collection.Apellido;
                i.DNI = collection.DNI;
                i.Telefono = collection.Telefono;
                reInq.Modificacion(i);
                TempData["Mensaje"] = "Datos guardados correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Inquilinos/Delete/5
        public ActionResult Delete(int id)
        {
            var inq = reInq.GetInquilino(id);
            return View(inq);
        }

        // POST: Inquilinos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(int id)
        {
            try
            {
                // TODO: Add delete logic here
                reInq.Baja(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
