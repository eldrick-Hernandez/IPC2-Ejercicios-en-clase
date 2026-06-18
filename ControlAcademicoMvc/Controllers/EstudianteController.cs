using Microsoft.AspNetCore.Mvc;
using ControlAcademicoMvc.Models;
using System.Collections.Generic;

namespace ControlAcademicoMvc.Controllers;

public class EstudianteController : Controller
{
    // Base de datos simulada en memoria
    private static readonly List<Estudiante> _baseDatosMemoria = new()
    {
        new Estudiante { Carne = 2026012, Nombre = "Fernando Velásquez", Promedio = 91.5 },
        new Estudiante { Carne = 2026045, Nombre = "Maria Mercedes", Promedio = 84.0 }
    };

    // GET: /Estudiante/Listar
    [HttpGet]
    public IActionResult Listar()
    {
        return Ok(_baseDatosMemoria);
    }

    // POST: /Estudiante/Registrar
    [HttpPost]
    public IActionResult Registrar([FromBody] Estudiante nuevoEstudiante)
    {
        if (nuevoEstudiante.Carne <= 0 || string.IsNullOrEmpty(nuevoEstudiante.Nombre))
        {
            return BadRequest(new { mensaje = "Datos del estudiante inválidos." });
        }
        
        _baseDatosMemoria.Add(nuevoEstudiante);
        return Created($"/Estudiante/Historial/{nuevoEstudiante.Carne}", nuevoEstudiante);
    }
}