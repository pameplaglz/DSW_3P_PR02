using System.Reflection;
using DSW_3P_PR02.Data;
using DSW_3P_PR02.Repositories;
using DSW_3P_PR02.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Registro de DbContext (SQL Server) y Repository (Versión 2: EF Core) + Service
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IEventoRepository, EfEventoRepository>();
builder.Services.AddScoped<IEventoService, EventoService>();

// Configuración de Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "DSW_3P_PR02 - Gestor de Eventos Tecnológicos v1");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
