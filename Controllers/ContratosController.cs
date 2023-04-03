using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public ContratosController()
        {
            repositorioInmueble = new RepositorioInmueble();
            repositorioContrato = new RepositorioContrato();
            repositorioInquilino = new RepositorioInquilino();
            repositorioPropietario = new RepositorioPropietario();

        }
        // GET: Contratos
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
        public ActionResult Details(int id)
        {
            var contrato = repositorioContrato.GetContrato(id);
            return View(contrato);
        }

        // GET: Contratos/Create
        public ActionResult Create()
        {
            ViewBag.Inquilinos = repositorioInquilino.GetInquilinos();
            ViewBag.Inmuebles = repositorioInmueble.GetInmuebles();
            return View();
        }

        // POST: Contratos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contrato collection)
        {
            try
            {
                // TODO: Add insert logic here
                repositorioContrato.Alta(collection);
                TempData["Id"] = collection.Id;
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Contratos/Edit/5
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