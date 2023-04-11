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
            var lista = repositorioContrato.GetContratos();
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            return View(lista);
        }

        // GET: Contratos/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            var contrato = repositorioContrato.GetContrato(id);
            ViewBag.InquilinoActual = repositorioInquilino.GetInquilino(contrato.InquilinoId);
            ViewBag.InmuebleActual = repositorioInmueble.GetInmueble(contrato.InmuebleId);
            return View(contrato);
        }

        // GET: Contratos/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Inquilinos = repositorioInquilino.GetInquilinos();
            ViewBag.Inmuebles = repositorioInmueble.GetInmueblesDisponibles();
            return View();
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
                TempData["Id"] = collection.Id;
                if (repositorioContrato.Alta(collection) > 0)
                {
                    var fechai = collection.FechaInicio;
                    var fechaf = collection.FechaFin;
                    var diferencia = fechaf.Subtract(fechai);
                    var meses = diferencia.Days / 30;
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

        // GET: Contratos/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            var contrato = repositorioContrato.GetContrato(id);
            ViewBag.Inquilinos = repositorioInquilino.GetInquilinos();
            ViewBag.InquilinoActual = repositorioInquilino.GetInquilino(contrato.InquilinoId);
            ViewBag.Inmuebles = repositorioInmueble.GetInmuebles();
            ViewBag.InmuebleActual = repositorioInmueble.GetInmueble(contrato.InmuebleId);
            return View(contrato);
        }

        // POST: Contratos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Contrato collection)
        {
            Contrato contrato = new Contrato();
            try
            {
                contrato = repositorioContrato.GetContrato(id);
                contrato.FechaInicio = collection.FechaInicio;
                contrato.FechaFin = collection.FechaFin;
                contrato.Precio = collection.Precio;
                contrato.InquilinoId = collection.InquilinoId;
                contrato.InmuebleId = collection.InmuebleId;
                contrato.Id = id;
                repositorioContrato.Modificacion(contrato);
                TempData["Mensaje"] = "Datos guardados correctamente";
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
                repositorioContrato.Baja(id);
                TempData["Mensaje"] = "Eliminación realizada correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}