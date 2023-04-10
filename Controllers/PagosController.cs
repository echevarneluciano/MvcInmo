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
        public ActionResult Index()
        {

            var lista = rePago.ObtenerTodos();
            return View(lista);
        }

        // GET: Pagos/Details/5
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
        public ActionResult Edit(int id)
        {
            var lista = rePago.ObtenerPorId(id);
            return View(lista);
        }

        // POST: Pagos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Pago collection)
        {
            Pago pago = new Pago();
            try
            {
                // TODO: Add update logic here
                pago = rePago.ObtenerPorId(collection.Id);
                pago.FechaPagado = collection.FechaPagado;
                rePago.Modificacion(pago);
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
        public ActionResult Eliminar(int id)
        {
            try
            {
                // TODO: Add delete logic here
                rePago.Baja(id);
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