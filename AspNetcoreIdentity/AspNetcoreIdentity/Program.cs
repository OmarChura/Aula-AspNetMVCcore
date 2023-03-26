using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AspNetcoreIdentity.Areas.Identity.Data;
using AspNetcoreIdentity.Extensions;
using AspNetcoreIdentity.Configurations;
using Microsoft.AspNetCore.Authorization;
using System.Configuration;
using KissLog.AspNetCore;
using KissLog;
using KissLog.CloudListeners.RequestLogsListener;
using KissLog.CloudListeners.Auth;

var builder = WebApplication.CreateBuilder(args);

//configuração do json
builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();


// *** Configurando servi�os no container ***

// Politica de cookies (LGPD)
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});


//var connectionString = builder.Configuration.GetConnectionString("AspNetcoreIdentityContextConnection") ?? throw new InvalidOperationException("Connection string 'AspNetcoreIdentityContextConnection' not found.");
builder.Services.AddIdentityConfig(builder.Configuration);
// Adicionando suporte ao contexto do Identity via EF
//builder.Services.AddDbContext<AspNetcoreIdentityContext>(options =>
//    options.UseSqlServer(connectionString));
// Adicionando suporte ao contexto do Identity via EF
//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>()
//    .AddEntityFrameworkStores<AspNetcoreIdentityContext>();

builder.Services.AddAuthorizationConfig();
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("PodeExcluir", policy => policy.RequireClaim("PodeExcluir"));
//
//    options.AddPolicy("PodeLer", policy => policy.Requirements.Add(new PermissaoNecesaria("PodeLer")));
//    options.AddPolicy("PodeEscrever", policy => policy.Requirements.Add(new PermissaoNecesaria("PodeEscrever")));
//});

builder.Services.ResolveDependencies();
//builder.Services.AddSingleton<IAuthorizationHandler, PermissaoNecessariaHandler>();


// Adicionando Autoriza��es personalizadas por policies
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("PodeExcluir", policy => policy.RequireClaim("PodeExcluir"));
//
//    options.AddPolicy("PodeLer", policy => policy.Requirements.Add(new PermissaoNecessaria("PodeLer")));
//    options.AddPolicy("PodeEscrever", policy => policy.Requirements.Add(new PermissaoNecessaria("PodeEscrever")));
//});

// Resolvendo DI para o Handler de Authorization
//builder.Services.AddSingleton<IAuthorizationHandler, PermissaoNecessariaHandler>();

// Extension Method de configuração KissLog
builder.Services.RegisterKissLogListeners();

// Add services to the container.
builder.Services.AddControllersWithViews(options => options.Filters.Add(typeof(AuditoriaFilter)));

// Adicionando suporte a componentes Razor (ex. Telas do Identity)
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    //app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/erro/500");
    app.UseStatusCodePagesWithRedirects("/erro/{0}");
    app.UseHsts();
}

if (app.Environment.IsProduction())
{
    builder.Configuration.AddUserSecrets<Program>();
}


// Redirecionamento para HTTPs
app.UseHttpsRedirection();

// Uso de arquivos est�ticos (ex. CSS, JS)
app.UseStaticFiles();

// Adicionando suporte a rota
app.UseRouting();

// Autenticacao e autoriza��o(Identity)
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.RegisterKissLogListeners(builder.Configuration);


// Mapeando componentes Razor Pages (ex: Identity)
app.MapRazorPages();


app.Run();


