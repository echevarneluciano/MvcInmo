using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using MvcInmo.Models;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/Usuarios/Login";
    options.LogoutPath = "/Usuarios/Logout";
    options.AccessDeniedPath = "/Home/Restringido";
})
.AddJwtBearer(options =>//la api web valida con token
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "inmobiliariaULP",//configuration["TokenAuthentication:Issuer"],
            ValidAudience = "mobileAPP",//configuration["TokenAuthentication:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(
                "Super_Secreta_es_la_clave_de_esta_APP_shhh")),//configuration["TokenAuthentication:SecretKey"]
        };
        // opción extra para usar el token en el hub y otras peticiones sin encabezado (enlaces, src de img, etc.)
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                // Leer el token desde el query string
                var accessToken = context.Request.Query["access_token"];
                // Si el request es para el Hub u otra ruta seleccionada...
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                    (path.StartsWithSegments("/chatsegurohub") ||
                    path.StartsWithSegments("/api/propietarios/reset") ||
                    path.StartsWithSegments("/api/propietarios/token")))
                {//reemplazar las urls por las necesarias ruta ⬆
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization(options =>
{
    //options.AddPolicy("Empleado", policy => policy.RequireClaim(ClaimTypes.Role, "Administrador", "Empleado"));
    options.AddPolicy("Administrador", policy => policy.RequireRole("Administrador", "SuperAdministrador"));
});

builder.Services.AddDbContext<DataContext>(
    options => options.UseMySql(
        "Server=localhost;User=root;Password=;Database=inmobiliaria;SslMode=none",
        ServerVersion.AutoDetect("Server=localhost;User=root;Password=;Database=inmobiliaria;SslMode=none")
    )
);


var app = builder.Build();

//builder.WebHost.UseUrls("http://localhost:5200", "http://*:5500", "http://10.120.10.172:5200");//permite escuchar peticiones locales y remotas 
builder.WebHost.UseUrls("http://localhost:5200", "http://*:5500", "http://10.120.10.172:5200");
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
