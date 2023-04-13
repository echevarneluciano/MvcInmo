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
    public class PropietariosController : Controller
    {
        private readonly RepositorioPropietario reProp;

        public PropietariosController()
        {
            reProp = new RepositorioPropietario();
        }
        // GET: Propietarios
        [Authorize]
        public ActionResult Index()
        {
            var lista = reProp.GetPropietarios();
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            return View(lista);
        }

        // GET: Propietarios/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            var propietario = reProp.GetPropietario(id);
            return View(propietario);
        }

        // GET: Propietarios/Create
        [Authorize]
        public ActionResult Create()
        {
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            return View();
        }

        // POST: Propietarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Propietario propietario)
        {

            try
            {
                // TODO: Add insert logic here
                if (propietario.Nombre == null || propietario.Apellido == null || propietario.DNI == null
                || propietario.Telefono == null || propietario.Email == null)
                {
                    TempData["Mensaje"] = "Debe llenar todos los campos";
                    return RedirectToAction(nameof(Create));
                }
                if (reProp.Alta(propietario) > 0)
                {
                    TempData["Mensaje"] = "Alta realizada correctamente. id: " + propietario.Id;
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Propietarios/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            var prop = reProp.GetPropietario(id);
            return View(prop);
        }

        // POST: Propietarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Propietario collection) //, Propietario collection
        {
            Propietario p = new Propietario();
            try
            {
                p = reProp.GetPropietario(id);
                p.Nombre = collection.Nombre;
                p.Apellido = collection.Apellido;
                p.DNI = collection.DNI;
                p.Telefono = collection.Telefono;
                p.Email = collection.Email;
                if (reProp.Modificacion(p) > 0)
                {
                    TempData["Mensaje"] = "Datos guardados correctamente";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {//poner breakpoints para detectar errores
                throw;
            }
        }

        // GET: Propietarios/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            var prop = reProp.GetPropietario(id);
            return View(prop);
        }

        // POST: Propietarios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Eliminar(int id)
        {
            try
            {
                // TODO: Add delete logic here
                if (reProp.Baja(id) > 0)
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