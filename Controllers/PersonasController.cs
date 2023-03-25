using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MvcInmo.Models;

namespace MvcInmo.Controllers;

public class PersonasController : Controller
{
    public PersonasController()
    {
    }

    public IActionResult Index()
    {
        RepositorioPersona repositorioPersona = new RepositorioPersona();
        var lista = repositorioPersona.GetPersonas();
        return View(lista);
    }
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Create(Persona persona)
    {
        RepositorioPersona repositorioPersona = new RepositorioPersona();
        int res = repositorioPersona.Alta(persona);
        if (res > 0)
        {
            return RedirectToAction("Index");
        }
        else
        {
            return View(persona);
        }
    }
}
