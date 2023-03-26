

using DevIO.UI.Site.Data;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

// *** Configurando serviços no container ***

// Politica de cookies (LGPD)
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    options.AreaPageViewLocationFormats.Clear();
    options.AreaPageViewLocationFormats.Add("/Modulos/{2}/Views/{1}/{0}.cshtml");
    options.AreaPageViewLocationFormats.Add("/Modulos/{2}/Views/Shared/{0}.cshtml");
    options.AreaPageViewLocationFormats.Add("/Views/Shared/{0}.cshtml");

});



//builder.Services.AddTransient<IPedidoRepository, PedidoRepository>();

//builder.Services.AddTransient<IOperacaoTransient, Operacao>();
//builder.Services.AddScoped<IOperacaoScoped, Operacao>();
//builder.Services.AddSingleton<IOperacaoSingleton, Operacao>();
//builder.Services.AddSingleton<IOperacaoSingletonInstance>(new Operacao(Guid.Empty));
//builder.Services.AddTransient<OperacaoService>();


IConfiguration configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;

// Adicionando suporte ao contexto do Identity via EF
builder.Services.AddDbContext<MeuDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("MeuDbContext")));

// Adicionando configuração padrão do Identity
//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
// .AddEntityFrameworkStores<AspNetCoreIdentityContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
