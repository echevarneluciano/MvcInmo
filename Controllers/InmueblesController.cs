using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcInmo.Models;

namespace MvcInmo.Controllers
{
    public class InmueblesController : Controller
    {
        // GET: Inmuebles
        private RepositorioInmueble repositorioInmueble;
        private RepositorioPropietario repositorioPropietario;
        public InmueblesController()
        {
            repositorioInmueble = new RepositorioInmueble();
            repositorioPropietario = new RepositorioPropietario();
        }
        public ActionResult Index()
        {
            var lista = repositorioInmueble.GetInmuebles();
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            return View(lista);
        }

        // GET: Inmuebles/Details/5
        public ActionResult Details(int id)
        {
            var inmueble = repositorioInmueble.GetInmueble(id);
            return View(inmueble);
        }

        // GET: Inmuebles/Create
        public ActionResult Create()
        {
            try
            {
                ViewBag.Propietarios = repositorioPropietario.GetPropietarios();
                return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // POST: Inmuebles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inmueble entidad)
        {
            try
            {
                ViewBag.Propietarios = repositorioPropietario.GetPropietarios();
                repositorioInmueble.Alta(entidad);
                TempData["Id"] = entidad.Id;
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(entidad);
            }
        }

        // GET: Inmuebles/Edit/5
        public ActionResult Edit(int id)
        {
            var inmueble = repositorioInmueble.GetInmueble(id);
            ViewBag.Propietarios = repositorioPropietario.GetPropietarios();
            ViewBag.PropietarioActual = repositorioPropietario.GetPropietario(inmueble.PropietarioId);
            return View(inmueble);
        }

        // POST: Inmuebles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Inmuebles/Delete/5
        public ActionResult Delete(int id)
        {
            var inmueble = repositorioInmueble.GetInmueble(id);
            return View(inmueble);
        }

        // POST: Inmuebles/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(int id)
        {
            try
            {
                // TODO: Add delete logic here
                repositorioInmueble.Baja(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}