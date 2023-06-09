using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcInmo.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace MvcInmo.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly RepositorioUsuario repositorioUsuario;
        private readonly IWebHostEnvironment environment;
        private readonly IConfiguration configuration;
        public UsuariosController(IWebHostEnvironment environment, IConfiguration configuration)
        {
            repositorioUsuario = new RepositorioUsuario();
            environment = environment;
            configuration = configuration;
        }
        // GET: Usuarios
        [Authorize(Policy = "Administrador")]
        public ActionResult Index()
        {
            try
            {
                if (TempData.ContainsKey("Mensaje"))
                {
                    ViewBag.Mensaje = TempData["Mensaje"];
                }
                var lista = repositorioUsuario.ObtenerTodos();
                return View(lista);
            }
            catch (Exception ex)
            {

                return View();
            }

        }

        // GET: Usuarios/Details/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Usuarios/Create
        [Authorize(Policy = "Administrador")]
        public ActionResult Create()
        {
            ViewBag.Roles = Usuario.ObtenerRoles();
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Create(Usuario u)
        {
            try
            {
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                password: u.Clave,
                                salt: System.Text.Encoding.ASCII.GetBytes("complicada"),
                                prf: KeyDerivationPrf.HMACSHA1,
                                iterationCount: 1000,
                                numBytesRequested: 256 / 8));
                u.Clave = hashed;
                u.Rol = User.IsInRole("Administrador") ? u.Rol : (int)enRoles.Empleado;
                int res = repositorioUsuario.Alta(u);
                if (u.AvatarFile != null && u.Id > 0)
                {
                    //string wwwPath = environment.WebRootPath;
                    string path = @"wwwroot\Uploads";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    //Path.GetFileName(u.AvatarFile.FileName);//este nombre se puede repetir
                    var nbreRnd = Guid.NewGuid();
                    string fileName = "avatar_" + u.Id + nbreRnd + Path.GetExtension(u.AvatarFile.FileName);
                    string pathCompleto = Path.Combine(path, fileName);
                    u.Avatar = @"/Uploads/" + fileName; //Path.Combine("Uploads", fileName);
                    // Esta operación guarda la foto en memoria en la ruta que necesitamos
                    using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                    {
                        u.AvatarFile.CopyTo(stream);
                    }
                    repositorioUsuario.Modificacion(u);
                }
                if (res > 0)
                {
                    TempData["Mensaje"] = "Usuario creado correctamente";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Roles = Usuario.ObtenerRoles();
                return View();
            }
        }

        // GET: Usuarios/Edit/5
        [Authorize]
        public ActionResult Perfil()
        {
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            ViewData["Title"] = "Mi perfil";
            var u = repositorioUsuario.ObtenerPorEmail(User.Identity.Name);
            ViewBag.Roles = Usuario.ObtenerRoles();
            return View("Perfil", u);
        }

        // GET: Usuarios/Edit/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Edit(int id)
        {
            ViewData["Title"] = "Editar usuario";
            var u = repositorioUsuario.ObtenerPorId(id);
            ViewBag.Roles = Usuario.ObtenerRoles();
            return View(u);
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Edit(int id, Usuario form1)
        {
            Usuario u = new Usuario();
            try
            {
                u = repositorioUsuario.ObtenerPorId(id);
                u.Nombre = form1.Nombre;
                u.Apellido = form1.Apellido;
                var cambiaMail = (form1.Email != u.Email) ? true : false;
                u.Email = form1.Email;
                u.Rol = form1.Rol;
                if (repositorioUsuario.Modificacion(u) > 0 && cambiaMail)
                {
                    var identity = (ClaimsIdentity)User.Identity;
                    identity.RemoveClaim(identity.FindFirst(ClaimTypes.Name));
                    identity.AddClaim(new Claim(ClaimTypes.Name, form1.Email));
                    await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));
                }

                if (form1.AvatarFile != null && u.Id > 0)
                {
                    var ruta = @"wwwroot" + u.Avatar;
                    if (System.IO.File.Exists(ruta))
                    {
                        System.IO.File.Delete(ruta);
                    }
                    var nbreRnd = Guid.NewGuid();
                    string path = @"wwwroot\Uploads";
                    string fileName = "avatar_" + u.Id + nbreRnd + Path.GetExtension(form1.AvatarFile.FileName);
                    string pathCompleto = Path.Combine(path, fileName);
                    u.Avatar = @"/Uploads/" + fileName;
                    using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                    {
                        form1.AvatarFile.CopyTo(stream);
                    }
                    repositorioUsuario.Modificacion(u);
                }

                if (form1.Clave != null && User.IsInRole("Administrador"))
                {
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                password: form1.Clave,
                                salt: System.Text.Encoding.ASCII.GetBytes("complicada"),
                                prf: KeyDerivationPrf.HMACSHA1,
                                iterationCount: 1000,
                                numBytesRequested: 256 / 8));
                    u.Clave = hashed;
                    repositorioUsuario.Modificacion(u);
                }

                TempData["Mensaje"] = "Datos guardados correctamente";
                if (!User.IsInRole("Administrador")) { return RedirectToAction(nameof(Perfil)); }
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {//colocar breakpoints en la siguiente línea por si algo falla
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult CambiarPass(int id, String claveNueva, String clave, String confirmarNueva)
        {
            Usuario u = new Usuario();
            string hashed;
            try
            {
                if (clave == null)
                {
                    TempData["Mensaje"] = "La contraseña no puede estar vacía";
                    return RedirectToAction(nameof(Perfil));
                }

                u = repositorioUsuario.ObtenerPorId(id);
                if (claveNueva == confirmarNueva)
                {
                    hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                                    password: claveNueva,
                                                    salt: System.Text.Encoding.ASCII.GetBytes("complicada"),
                                                    prf: KeyDerivationPrf.HMACSHA1,
                                                    iterationCount: 1000,
                                                    numBytesRequested: 256 / 8));
                    claveNueva = hashed;
                }
                else
                {
                    TempData["Mensaje"] = "Las contraseñas no coinciden";
                    return RedirectToAction(nameof(Perfil));
                }

                hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                                 password: clave,
                                                 salt: System.Text.Encoding.ASCII.GetBytes("complicada"),
                                                 prf: KeyDerivationPrf.HMACSHA1,
                                                 iterationCount: 1000,
                                                 numBytesRequested: 256 / 8));
                clave = hashed;

                if (clave == u.Clave)
                {
                    u.Clave = claveNueva;
                    repositorioUsuario.Modificacion(u);
                    TempData["Mensaje"] = "Contraseña actualizada";
                }
                else
                {
                    TempData["Mensaje"] = "Contraseña actual invalida";
                    return RedirectToAction(nameof(Perfil));
                }

                return RedirectToAction(nameof(Perfil));
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        // GET: Usuarios/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            var u = repositorioUsuario.ObtenerPorId(id);
            return View(u);
        }

        // POST: Usuarios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Eliminar(int id)
        {
            try
            {
                // TODO: Add delete logic here
                var u = repositorioUsuario.ObtenerPorId(id);
                var ruta = @"wwwroot" + u.Avatar;

                if (repositorioUsuario.Baja(id) > 0)
                {
                    if (System.IO.File.Exists(ruta))
                    {
                        System.IO.File.Delete(ruta);
                    }
                    TempData["Mensaje"] = "Eliminación realizada correctamente";
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            TempData["returnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginView login)
        {
            try
            {
                var returnUrl = String.IsNullOrEmpty(TempData["returnUrl"] as string) ? "/Home" : TempData["returnUrl"].ToString();
                if (ModelState.IsValid)
                {
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: login.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes("complicada"),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));

                    var e = repositorioUsuario.ObtenerPorEmail(login.Usuario);
                    if (e == null || e.Clave != hashed)
                    {
                        ModelState.AddModelError("", "El email o la clave no son correctos");
                        TempData["returnUrl"] = returnUrl;
                        return View();
                    }

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, e.Email),
                        new Claim("FullName", e.Nombre + " " + e.Apellido),
                        new Claim(ClaimTypes.Role, e.RolNombre),
                    };

                    var claimsIdentity = new ClaimsIdentity(
                            claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity));
                    TempData.Remove("returnUrl");
                    return Redirect(returnUrl);
                }
                TempData["returnUrl"] = returnUrl;
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // GET: /salir
        [Route("salir", Name = "logout")]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["Mensaje"] = "Hasta pronto";
            return RedirectToAction("Index", "Home");
        }

    }
}