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
    public class PagosController : Controller
    {
        private readonly RepositorioPago rePago;
        private readonly RepositorioContrato reContrato;
        public PagosController()
        {
            rePago = new RepositorioPago();
            reContrato = new RepositorioContrato();
        }
        // GET: Pagos
        [Authorize]
        public ActionResult Index()
        {
            if (TempData.ContainsKey("Mensaje"))
            {
                ViewBag.Mensaje = TempData["Mensaje"];
            }
            var lista = rePago.ObtenerTodos();
            return View(lista);
        }

        // GET: Pagos/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            var lista = rePago.ObtenerPorId(id);
            return View(lista);
        }

        // GET: Pagos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pagos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Pagos/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            var lista = rePago.ObtenerPorId(id);
            return View(lista);
        }

        // POST: Pagos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(Pago collection)
        {
            Pago pago = new Pago();
            try
            {
                // TODO: Add update logic here
                if (collection.FechaPagado == null)
                {
                    TempData["Mensaje"] = "Debe ingresar la fecha de pago";
                    return RedirectToAction(nameof(Edit), new { id = collection.Id });
                }
                if (collection.Importe <= 0)
                {
                    TempData["Mensaje"] = "Debe ingresar un importe mayor a cero";
                    return RedirectToAction(nameof(Edit), new { id = collection.Id });
                }
                pago = rePago.ObtenerPorId(collection.Id);
                pago.FechaPagado = collection.FechaPagado;
                pago.Importe = collection.Importe;
                if (rePago.Modificacion(pago) > 0)
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

        // GET: Pagos/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            var lista = rePago.ObtenerPorId(id);
            return View(lista);
        }

        // POST: Pagos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Eliminar(int id)
        {
            try
            {
                // TODO: Add delete logic here
                if (rePago.Baja(id) > 0)
                {
                    TempData["Mensaje"] = "Datos eliminados correctamente";
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        /*         // POST: Pagos/Buscar
                [HttpPost]
                [ValidateAntiForgeryToken]
                public ActionResult Buscar(int id)
                {
                    try
                    {
                        // TODO: Add insert logic here
                        var lista = rePago.ObtenerPorContrato(id);
                        return Json(lista);
                    }
                    catch
                    {
                        return View();
                    }
                } */

    }
}