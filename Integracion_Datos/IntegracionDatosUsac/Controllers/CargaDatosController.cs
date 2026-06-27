using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CargaDatosController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CargaDatosController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("carga-masiva")]
    public async Task<IActionResult> CargarArchivoCsvMasivo(IFormFile archivo)
    {
        if (archivo == null || archivo.Length == 0)
        {
            return BadRequest("El archivo proporcionado no es válido o está vacío.");
        }

        var listaIntermedia = new List<RegistroAlumno>();

        using (var stream = archivo.OpenReadStream())
        using (var reader = new StreamReader(stream))
        {
            string linea;
            
            // Lectura línea por línea de manera asíncrona para evitar saturar la RAM
            while ((linea = await reader.ReadLineAsync()) != null)
            {
                var valores = linea.Split(',');

                // Lógica de mapeo básico asumiendo que el CSV tiene Carnet y Nombre
                if (valores.Length >= 2)
                {
                    listaIntermedia.Add(new RegistroAlumno
                    {
                        Carnet = valores[0],
                        Nombre = valores[1]
                    });
                }
            }
        }

        // Aplicación del patrón Batching: Inserción completa en la base de datos
        // en un solo bloque mediante AddRange y SaveChangesAsync
        _context.RegistrosAlumnos.AddRange(listaIntermedia);
        await _context.SaveChangesAsync();

        return Ok(new { Mensaje = $"Proceso finalizado. Se cargaron {listaIntermedia.Count} registros correctamente." });
    }
    // Clases falsas (Dummy) solo para que compile el código
        public class ApplicationDbContext
        {
            public DbSet<RegistroAlumno> RegistrosAlumnos { get; set; }
            public Task SaveChangesAsync() => Task.CompletedTask;
        }

        public class DbSet<T>
        {
            public void AddRange(IEnumerable<T> entities) { }
        }

        public class RegistroAlumno
        {
            public string Carnet { get; set; }
            public string Nombre { get; set; }
        }
}