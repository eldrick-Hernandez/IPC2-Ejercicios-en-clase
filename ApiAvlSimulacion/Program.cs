using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// 1. Almacenamiento en memoria (Va ANTES de la clase)
var estadoArbol = new List<NodoAVL>
{
    new NodoAVL { Id = 30, Etiqueta = "Nodo Raíz (Abuelo) - FE: -2" },
    new NodoAVL { Id = 10, Etiqueta = "Hijo Izquierdo - FE: +1" }
};

// 2. ENDPOINT GET
app.MapGet("/api/arbol", () => Results.Ok(estadoArbol));

// 3. ENDPOINT POST
app.MapPost("/api/arbol/insertar", (NodoAVL nuevoNodo) =>
{
    // Validación básica
    if (nuevoNodo.Id <= 0) return Results.BadRequest("ID de nodo inválido.");

    // Lógica de Rotación RID (al insertar el 20)
    if (nuevoNodo.Id == 20)
    {
        estadoArbol.Clear();
        estadoArbol.Add(new NodoAVL { Id = 20, Etiqueta = "Nueva Raíz Balanceada (RID) - FE: 0" });
        estadoArbol.Add(new NodoAVL { Id = 10, Etiqueta = "Hijo Izquierdo - FE: 0" });
        estadoArbol.Add(new NodoAVL { Id = 30, Etiqueta = "Hijo Derecho - FE: 0" });
        
        return Results.Created("/api/arbol", new { 
            Mensaje = "Rotación RID ejecutada con éxito. Estabilidad total lograda.", 
            Estructura = estadoArbol 
        });
    }

    // Inserción tradicional
    estadoArbol.Add(nuevoNodo);
    return Results.Created($"/api/arbol/{nuevoNodo.Id}", nuevoNodo);
});

// ¡app.Run() siempre debe ser la última instrucción de ejecución!
app.Run();


// 4. Modelo del Nodo (Las clases van HASTA EL FINAL)
public class NodoAVL 
{
    public int Id { get; set; } // Actúa como el Dato/Llave
    public string Etiqueta { get; set; } = string.Empty;
    public int Altura { get; set; } = 1;
}