using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public ActionResult Index()
        {
            var lista = reProp.GetPropietarios();
            return View(lista);
        }

        // GET: Propietarios/Details/5
        public ActionResult Details(int id)
        {
            var propietario = reProp.GetPropietario(id);
            return View(propietario);
        }

        // GET: Propietarios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Propietarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Propietario propietario)
        {

            try
            {
                // TODO: Add insert logic here
                reProp.Alta(propietario);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Propietarios/Edit/5
        public ActionResult Edit(int id)
        {
            var prop = reProp.GetPropietario(id);
            return View(prop);
        }

        // POST: Propietarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
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
                reProp.Modificacion(p);
                TempData["Mensaje"] = "Datos guardados correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {//poner breakpoints para detectar errores
                throw;
            }
        }

        // GET: Propietarios/Delete/5
        public ActionResult Delete(int id)
        {
            var prop = reProp.GetPropietario(id);
            return View(prop);
        }

        // POST: Propietarios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(int id)
        {
            try
            {
                // TODO: Add delete logic here
                reProp.Baja(id);
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