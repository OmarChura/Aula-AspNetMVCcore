using AspNetCoreIdentity.Config;
using AspNetCoreIdentity.Extensions;
using KissLog.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

#region " Configurando serviços no container "

builder.Services.AddIdentityConfig(builder.Configuration);
builder.Services.AddAuthorizationConfig();
builder.Services.ResolveDependencies();
builder.Services.RegisterKissLogListeners();

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(typeof(AuditoriaFilter));
});

builder.Services.AddRazorPages();

var app = builder.Build();

#endregion

# region " Configurando o resquest dos serviços no pipeline "

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/erro/500");
    app.UseStatusCodePagesWithRedirects("/erro/{0}");
    app.UseHsts();
}

app.UseKissLogMiddleware();

app.UseHttpsRedirection();
app.UseStaticFiles();

// Adicionando suporte a rota
app.UseRouting();

// Autenticacao e autorização (Identity)
app.UseAuthentication();
app.UseAuthorization();

// Rota padrão
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

// Mapeando componentes Razor Pages (ex: Identity)
app.MapRazorPages();

app.RegisterKissLogListeners(builder.Configuration);

app.Run();

#endregion