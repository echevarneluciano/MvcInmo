using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public ActionResult Index()
        {
            var lista = reInq.GetInquilinos();
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            return View(lista);
        }

        // GET: Inquilinos/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            var inquilino = reInq.GetInquilino(id);
            return View(inquilino);
        }

        // GET: Inquilinos/Create
        [Authorize]
        public ActionResult Create()
        {
            if (TempData.ContainsKey("Mensaje"))
            {
                ViewBag.Mensaje = TempData["Mensaje"];
            }
            return View();
        }

        // POST: Inquilinos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Inquilino inquilino)
        {
            try
            {
                // TODO: Add insert logic here
                if (inquilino.Nombre == null || inquilino.Apellido == null || inquilino.DNI == null ||
                inquilino.Telefono == null || inquilino.Email == null)
                {
                    TempData["Mensaje"] = "Todos los campos son obligatorios";
                    return RedirectToAction(nameof(Create));
                }
                if (reInq.Alta(inquilino) > 0)
                {
                    TempData["Mensaje"] = "Alta realizada correctamente. id: " + inquilino.Id;
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Inquilinos/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            var inquilino = reInq.GetInquilino(id);
            return View(inquilino);
        }

        // POST: Inquilinos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
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
                i.Email = collection.Email;
                if (reInq.Modificacion(i) > 0)
                {
                    TempData["Mensaje"] = "Datos guardados correctamente";
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Inquilinos/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            var inq = reInq.GetInquilino(id);
            return View(inq);
        }

        // POST: Inquilinos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Eliminar(int id)
        {
            try
            {
                // TODO: Add delete logic here
                if (reInq.Baja(id) > 0)
                {
                    TempData["Mensaje"] = "Eliminación realizada correctamente";
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
