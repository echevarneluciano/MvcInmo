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

        [Authorize]
        public ActionResult Contrato(int id)
        {
            var lista = rePago.ObtenerPorContrato(id);
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            ViewBag.Contrato = id;
            return View("Index", lista);
        }


        // GET: Pagos/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            var deuda = rePago.ObtenerDeuda(id);
            ViewBag.Deuda = deuda;
            var lista = rePago.ObtenerPorId(id);
            return View(lista);
        }

        [Authorize]
        // GET: Pagos/Create
        public ActionResult Create(int id)
        {
            try
            {
                if (TempData.ContainsKey("Mensaje"))
                    ViewBag.Mensaje = TempData["Mensaje"];
                if (id == 0)
                {
                    ViewBag.Contratos = reContrato.GetContratos();
                }
                else
                {
                    ViewBag.Contrato = reContrato.GetContrato(id);
                }

                return View();
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        [Authorize]
        // POST: Pagos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pago collection)
        {
            Pago pago = new Pago();
            try
            {
                if (collection.FechaPagado == null)
                {
                    TempData["Mensaje"] = "Debe ingresar la fecha de pago";
                    return RedirectToAction(nameof(Create));
                }
                if (collection.Importe == null)
                {
                    TempData["Mensaje"] = "Debe ingresar un importe mayor a cero";
                    return RedirectToAction(nameof(Create));
                }
                if (collection.Mes == 0)
                {
                    TempData["Mensaje"] = "Debe ingresar N° de cuota";
                    return RedirectToAction(nameof(Create));
                }
                pago.FechaPagado = collection.FechaPagado;
                pago.Importe = collection.Importe;
                pago.ContratoId = (collection.ContratoId == 0) ? collection.Id : collection.ContratoId;
                pago.Mes = collection.Mes;
                if (rePago.Alta(pago) > 0)
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
    }
}