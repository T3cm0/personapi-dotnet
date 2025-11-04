using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using personapi_dotnet.Models.Entities;

// repos
using personapi_dotnet.Repositories;
using personapi_dotnet.Repositories.Interfaces;

// NSwag (cliente generado)
using personapi_dotnet.NSwagClient;

var builder = WebApplication.CreateBuilder(args);

// === MVC + Newtonsoft.Json (NSwag genera DTOs para Newtonsoft) ===
builder.Services
    .AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
        // Si quieres camelCase:
        // options.SerializerSettings.ContractResolver =
        //     new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
    });

// === DbContext (EF Core) ===
builder.Services.AddDbContext<PersonaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PersonaDb")));

// === Repositorios ===
builder.Services.AddScoped<IPersonaRepository, PersonaRepository>();

// === Swagger (UI) ===
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "personapi-dotnet", Version = "v1" });
});

// === Cliente NSwag apuntando a TU API ===
// Puedes sobreescribir en appsettings*.json:  "Api": { "BaseUrl": "http://localhost:5205" }
var baseUrl = builder.Configuration["Api:BaseUrl"] ?? "http://localhost:5205";

// Named HttpClient para el cliente generado por NSwag
builder.Services.AddHttpClient("nswag", c =>
{
    c.BaseAddress = new Uri(baseUrl);
});

// Registro del IApiClient usando la fábrica (ctor: ApiClient(string baseUrl, HttpClient http))
builder.Services.AddScoped<IApiClient>(sp =>
{
    var http = sp.GetRequiredService<IHttpClientFactory>().CreateClient("nswag");
    return new ApiClient(baseUrl, http);
});
// ================================================================

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "personapi-dotnet v1");
    });
}

app.UseStaticFiles();
app.UseRouting();

// API + MVC
app.MapControllers();

// Ruta MVC por convención (Persons/Index)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Persons}/{action=Index}/{id?}");

app.Run();
