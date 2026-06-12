using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Linq; // Necesario para usar .Take()

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


var coleccionNodos = new NodoElemento[100];
int cantidadActual = 0; // Llevamos el control de cuántos nodos hay realmente

// Insertamos los valores iniciales usando el índice
coleccionNodos[cantidadActual] = new NodoElemento { Id = 10, Valor = "Raíz Inicial (ABB)" };
cantidadActual++; // Incrementamos el contador

coleccionNodos[cantidadActual] = new NodoElemento { Id = 5, Valor = "Hijo Izquierdo" };
cantidadActual++;


app.MapGet("/api/nodos", () => {

    var nodosOcupados = coleccionNodos.Take(cantidadActual);
    return Results.Ok(nodosOcupados);
});

// POST: Inserta un nuevo nodo
app.MapPost("/api/nodos", (NodoElemento nuevoNodo) =>
{
    // Validación 1: ¿Queda espacio en el arreglo?
    if (cantidadActual >= coleccionNodos.Length)
    {
        return Results.BadRequest("El arreglo está lleno. No se pueden agregar más nodos.");
    }

    // Validación 2: Datos correctos
    if (nuevoNodo.Id <= 0 || string.IsNullOrEmpty(nuevoNodo.Valor))
    {
        return Results.BadRequest("Datos del nodo inválidos.");
    }
    
    // Insertamos en la posición libre actual y luego aumentamos el contador
    coleccionNodos[cantidadActual] = nuevoNodo;
    cantidadActual++;
    
    return Results.Created($"/api/nodos/{nuevoNodo.Id}", nuevoNodo);
});


app.Run();

public class NodoElemento
{
    public int Id { get; set; }
    public string Valor { get; set; } = string.Empty;
}