using Microsoft.EntityFrameworkCore;
using APIs.Data;

namespace APIs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configurar la cadena de conexi�n y el contexto de base de datos
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Agregar servicios para controladores
            builder.Services.AddControllers();

            // Configurar y construir la aplicaci�n
            var app = builder.Build();

            // Configurar el pipeline de la aplicaci�n
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}