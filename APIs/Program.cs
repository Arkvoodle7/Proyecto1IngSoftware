using Microsoft.EntityFrameworkCore;
using APIs.Data;
using Microsoft.OpenApi.Models;

namespace APIs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configuración de DbContext y conexión
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Agregar controladores y configurar Swagger
            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API de Productos y Venta", Version = "v1" });
            });

            var app = builder.Build();

            // Habilitar Swagger solo en desarrollo
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API de Productos v1");
                    //c.RoutePrefix = string.Empty; // Coloca Swagger en la raíz
                });
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
