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
        [Authorize]
        public ActionResult Index()
        {
            var lista = repositorioInmueble.GetInmuebles();
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            return View(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Disponibles(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var FechaInicio = fechaInicio;
                var FechaFin = fechaFin;

                var lista = repositorioInmueble.GetInmueblesDisponiblesPorFechas(FechaInicio, FechaFin);

                if (TempData.ContainsKey("Id"))
                    ViewBag.Id = TempData["Id"];
                if (TempData.ContainsKey("Mensaje"))
                    ViewBag.Mensaje = TempData["Mensaje"];
                if (lista.Count != 0)
                {
                    ViewBag.Mensaje = "Inmuebles disponibles entre: " + FechaInicio.ToString("dd/MM/yyyy") + " y " + FechaFin.ToString("dd/MM/yyyy");
                }
                return View("Index", lista);
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        [Authorize]
        public ActionResult Propietario(int id)
        {
            var lista = repositorioInmueble.BuscarPorPropietario(id);
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            ViewBag.Propietario = id;
            if (lista.Count != 0)
            {
                ViewBag.Mensaje = "Inmuebles del propietario: " + lista[0].Duenio.Nombre + " " + lista[0].Duenio.Apellido;
            }
            return View("Index", lista);
        }
        // GET: Inmuebles/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            var inmueble = repositorioInmueble.GetInmueble(id);
            return View(inmueble);
        }

        // GET: Inmuebles/Create
        [Authorize]
        public ActionResult Create(int id)
        {
            try
            {
                if (TempData.ContainsKey("Mensaje"))
                {
                    ViewBag.Mensaje = TempData["Mensaje"];
                }
                ViewBag.Propietarios = repositorioPropietario.GetPropietarios();
                ViewBag.Propietario = id;
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
        [Authorize]
        public ActionResult Create(Inmueble entidad)
        {
            try
            {
                if (entidad.Direccion == null || entidad.Ambientes == null
                || entidad.Superficie == null || entidad.Latitud == null
                || entidad.Precio == null || entidad.Longitud == null)
                {
                    TempData["Mensaje"] = "Debe llenar todos los campos";
                    return RedirectToAction(nameof(Create));
                }
                ViewBag.Propietarios = repositorioPropietario.GetPropietarios();
                if (repositorioInmueble.Alta(entidad) > 0)
                {
                    TempData["Mensaje"] = "Inmueble creado con exito. Id: " + entidad.Id;
                }
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
        [Authorize]
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
        [Authorize]
        public ActionResult Edit(int id, Inmueble collection)
        {
            Inmueble inmueble = new Inmueble();
            try
            {
                inmueble = repositorioInmueble.GetInmueble(id);
                inmueble.Ambientes = collection.Ambientes;
                inmueble.Direccion = collection.Direccion;
                inmueble.Id = collection.Id;
                inmueble.Latitud = collection.Latitud;
                inmueble.PropietarioId = collection.PropietarioId;
                inmueble.Longitud = collection.Longitud;
                inmueble.Superficie = collection.Superficie;
                inmueble.Tipo = collection.Tipo;
                inmueble.Precio = collection.Precio;
                inmueble.Estado = collection.Estado;
                if (repositorioInmueble.Modificacion(inmueble) > 0)
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

        // GET: Inmuebles/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            var inmueble = repositorioInmueble.GetInmueble(id);
            return View(inmueble);
        }

        // POST: Inmuebles/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Eliminar(int id)
        {
            try
            {
                // TODO: Add delete logic here
                if (repositorioInmueble.Baja(id) > 0)
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