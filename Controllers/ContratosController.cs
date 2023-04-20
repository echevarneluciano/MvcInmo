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
    public class ContratosController : Controller
    {
        private RepositorioInmueble repositorioInmueble;
        private RepositorioInquilino repositorioInquilino;
        private RepositorioContrato repositorioContrato;
        private RepositorioPropietario repositorioPropietario;
        private RepositorioPago repositorioPago;
        public ContratosController()
        {
            repositorioInmueble = new RepositorioInmueble();
            repositorioContrato = new RepositorioContrato();
            repositorioInquilino = new RepositorioInquilino();
            repositorioPropietario = new RepositorioPropietario();
            repositorioPago = new RepositorioPago();

        }
        // GET: Contratos
        [Authorize]
        public ActionResult Index()
        {
            try
            {
                var lista = repositorioContrato.GetContratos();
                if (TempData.ContainsKey("Id"))
                    ViewBag.Id = TempData["Id"];
                if (TempData.ContainsKey("Mensaje"))
                    ViewBag.Mensaje = TempData["Mensaje"];
                return View(lista);
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Vigentes(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var FechaInicio = fechaInicio;
                var FechaFin = fechaFin;

                var lista = repositorioContrato.GetContratosVigentes(FechaInicio, FechaFin);

                if (TempData.ContainsKey("Id"))
                    ViewBag.Id = TempData["Id"];
                if (TempData.ContainsKey("Mensaje"))
                    ViewBag.Mensaje = TempData["Mensaje"];
                return View("Index", lista);
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        [Authorize]
        public ActionResult Inmueble(int id)
        {
            var lista = repositorioContrato.GetContratosPorInmueble(id);
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            ViewBag.Inquilino = id;
            return View("Index", lista);
        }

        // GET: Contratos/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            try
            {
                var contrato = repositorioContrato.GetContrato(id);
                ViewBag.InquilinoActual = repositorioInquilino.GetInquilino(contrato.InquilinoId);
                ViewBag.InmuebleActual = repositorioInmueble.GetInmueble(contrato.InmuebleId);
                return View(contrato);
            }
            catch (System.Exception)
            {

                throw;
            }

        }

        // GET: Contratos/Create
        [Authorize]
        public ActionResult Create(int id)
        {
            try
            {
                if (TempData.ContainsKey("Id"))
                    ViewBag.Id = TempData["Id"];
                if (TempData.ContainsKey("Mensaje"))
                    ViewBag.Mensaje = TempData["Mensaje"];
                ViewBag.Inquilinos = repositorioInquilino.GetInquilinos();
                ViewBag.Inmuebles = repositorioInmueble.GetInmueblesDisponibles();
                ViewBag.Inmueble = id;
                return View();
            }
            catch (System.Exception)
            {

                throw;
            }

        }

        // POST: Contratos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Contrato collection)
        {
            try
            {
                // TODO: Add insert logic here
                Contrato controlFecha;
                controlFecha = repositorioContrato.compruebaFechas(collection.InmuebleId, collection.FechaInicio, collection.FechaFin);
                if (controlFecha != null)
                {
                    TempData["Mensaje"] = "El inmueble id: " + collection.InmuebleId + " tiene contrato en la fecha seleccionada";
                    return RedirectToAction(nameof(Create));
                }
                if (collection.FechaFin < collection.FechaInicio)
                {
                    TempData["Mensaje"] = "La fecha de fin debe ser mayor a la de inicio";
                    return RedirectToAction(nameof(Create));
                }
                if (collection.Precio == null)
                {
                    TempData["Mensaje"] = "El precio no puede ser nulo";
                    return RedirectToAction(nameof(Create));
                }
                if (repositorioContrato.Alta(collection) > 0)
                {
                    TempData["Mensaje"] = "Contrato creado con exito, id: " + collection.Id;
                    var fechai = collection.FechaInicio;
                    var fechaf = collection.FechaFin;
                    var diferencia = fechaf.Subtract(fechai);
                    var meses = diferencia.Days / 30;
                    if (meses == 0) { meses = 1; }
                    Console.WriteLine("Cantidad de meses: " + meses);
                    for (int i = 0; i < meses; i++)
                    {
                        var pago = new Pago();
                        pago.Mes = i;
                        pago.ContratoId = collection.Id;
                        repositorioPago.Alta(pago);
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [Authorize]
        public ActionResult Terminar(int id)
        {
            try
            {
                var deuda = repositorioPago.ObtenerDeuda(id);

                var contrato = repositorioContrato.GetContrato(id);
                ViewBag.InquilinoActual = repositorioInquilino.GetInquilino(contrato.InquilinoId);
                ViewBag.InmuebleActual = repositorioInmueble.GetInmueble(contrato.InmuebleId);
                ViewBag.deuda = deuda;

                var intervaloF = contrato.FechaFin.Subtract(contrato.FechaInicio) / 30;
                var intervaloA = DateTime.Now.Subtract(contrato.FechaInicio) / 30;
                if ((intervaloF / 2) > intervaloA)
                {
                    ViewBag.multa = contrato.Precio * 2;
                }

                return View(contrato);
            }
            catch (System.Exception)
            {

                throw;
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Terminar(int id, Contrato collection)
        {
            Contrato contrato = new Contrato();
            try
            {
                contrato = repositorioContrato.GetContrato(id);
                contrato.FechaFin = DateTime.Now;
                if (repositorioContrato.Modificacion(contrato) > 0)
                {
                    repositorioPago.ModificacionPorContratoId(contrato.Id, DateTime.Now);
                    TempData["Mensaje"] = "Contrato terminado con exito";
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Contratos/Delete/5
        [Authorize(Roles = "Administrador")]
        public ActionResult Delete(int id)
        {
            var contrato = repositorioContrato.GetContrato(id);
            ViewBag.InquilinoActual = repositorioInquilino.GetInquilino(contrato.InquilinoId);
            ViewBag.InmuebleActual = repositorioInmueble.GetInmueble(contrato.InmuebleId);
            return View(contrato);
        }

        // POST: Contratos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public ActionResult Eliminar(int id)
        {
            try
            {
                // TODO: Add delete logic here
                if (repositorioContrato.Baja(id) > 0)
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

        [Authorize]
        public ActionResult Renovar(int id)
        {
            try
            {
                if (TempData["Mensaje"] != null)
                {
                    ViewBag.Mensaje = TempData["Mensaje"];
                }
                var contrato = repositorioContrato.GetContrato(id);
                ViewBag.Inquilinos = repositorioInquilino.GetInquilinos();
                ViewBag.InquilinoActual = repositorioInquilino.GetInquilino(contrato.InquilinoId);
                ViewBag.Inmuebles = repositorioInmueble.GetInmuebles();
                ViewBag.InmuebleActual = repositorioInmueble.GetInmueble(contrato.InmuebleId);
                return View(contrato);
            }
            catch (System.Exception)
            {

                throw;
            }

        }

        // POST: Contratos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Renovar(int id, Contrato collection)
        {
            Contrato contrato = new Contrato();
            try
            {
                contrato = repositorioContrato.GetContrato(id);

                if (collection.Precio <= 0 || collection.FechaFin == null)
                {
                    TempData["Mensaje"] = "Complete campos fecha y precio";
                    return RedirectToAction(nameof(Renovar));
                }
                if (collection.FechaFin <= contrato.FechaFin)
                {
                    TempData["Mensaje"] = "La fecha de fin debe ser mayor a la fecha fin actual";
                    return RedirectToAction(nameof(Renovar));
                }

                var fechai = contrato.FechaFin;
                var fechaf = collection.FechaFin;
                var diferencia = fechaf.Subtract(fechai);
                var meses = diferencia.Days / 30;
                if (meses == 0) { meses = 1; }
                Console.WriteLine("Cantidad de meses: " + meses);

                contrato.FechaFin = collection.FechaFin;
                contrato.Precio = collection.Precio;

                if (repositorioContrato.Modificacion(contrato) > 0)
                {
                    for (int i = 0; i < meses; i++)
                    {
                        var pago = new Pago();
                        pago.Mes = i;
                        pago.ContratoId = collection.Id;
                        repositorioPago.Alta(pago);
                    }
                    TempData["Mensaje"] = "Datos guardados correctamente";
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