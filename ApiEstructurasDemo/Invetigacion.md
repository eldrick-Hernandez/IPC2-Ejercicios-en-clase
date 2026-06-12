# Actividad de Investigación y Práctica: Estructuras de Datos Avanzadas y APIs con ASP.NET Core

**Universidad de San Carlos de Guatemala** **Facultad de Ingeniería** **Laboratorio de Introducción a la Programación y Computación 2** **Vacaciones 1er Semestre 2026** **Auxiliar:** Fernando Jose Vicente Velasquez  

**Estudiante:** Eldrick Aldair Hernández Bautista  
**Carné:** 202403488  

---

## Parte 1: Investigación Teórica

### 1. Estructuras de Datos Eficientes

**Árboles Binarios de Búsqueda (ABB):** Su regla de ordenamiento establece que para cualquier nodo, los valores en su subárbol izquierdo son menores que el nodo raíz, y los del subárbol derecho son mayores. Su principal desventaja es que, si los datos se insertan en orden secuencial, el árbol no se balancea y degenera en una lista vinculada, perdiendo su eficiencia y pasando de un tiempo de búsqueda óptimo a una complejidad lineal de $O(n)$.

**Árboles AVL:** Es un árbol binario de búsqueda auto-balanceado, lo que significa que ajusta su estructura automáticamente al insertar o eliminar nodos para mantener una altura mínima. Su factor de balanceo se calcula como $Factor = Altura_{Izquierda} - Altura_{Derecha}$. Gracias a este factor, la diferencia de altura entre subárboles nunca excede 1, garantizando que el árbol se mantenga poco profundo y su complejidad para búsqueda, inserción y eliminación sea siempre $O(\log n)$.

### 2. Fundamentos de Web APIs

**API y Modelo Cliente-Servidor:** Una API es una interfaz que permite la comunicación entre sistemas. En el modelo Cliente-Servidor, el cliente envía una petición (Request) al servidor mediante el protocolo HTTP. El servidor procesa la solicitud y devuelve una respuesta (Response).

**Verbos HTTP:** * **GET:** Se utiliza para recuperar recursos. Es un método idempotente, lo que significa que realizar la misma petición varias veces producirá el mismo resultado sin modificar el estado del servidor.
* **POST:** Se usa para crear nuevos recursos. No es idempotente, ya que hacer la misma petición múltiples veces creará múltiples recursos nuevos en el servidor.

---

## Parte 2: Implementación Práctica en C# con ASP.NET Core

A continuación, se presenta el código de la API utilizando un **arreglo estático** para el almacenamiento en memoria, tal como se implementó en el archivo `Program.cs`:

```csharp
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// 3. Configuración del almacenamiento con ARREGLO ESTÁTICO
var coleccionNodos = new NodoElemento[100];
int cantidadActual = 0;

coleccionNodos[cantidadActual] = new NodoElemento { Id = 10, Valor = "Raíz Inicial (ABB)" };
cantidadActual++;

coleccionNodos[cantidadActual] = new NodoElemento { Id = 5, Valor = "Hijo Izquierdo" };
cantidadActual++;

// 4. Implementación de Endpoints
app.MapGet("/api/nodos", () => {
    var nodosOcupados = coleccionNodos.Take(cantidadActual);
    return Results.Ok(nodosOcupados);
});

app.MapPost("/api/nodos", (NodoElemento nuevoNodo) =>
{
    if (cantidadActual >= coleccionNodos.Length)
    {
        return Results.BadRequest("El arreglo está lleno. No se pueden agregar más nodos.");
    }
    if (nuevoNodo.Id <= 0 || string.IsNullOrEmpty(nuevoNodo.Valor))
    {
        return Results.BadRequest("Datos del nodo inválidos.");
    }
    
    coleccionNodos[cantidadActual] = nuevoNodo;
    cantidadActual++;
    
    return Results.Created($"/api/nodos/{nuevoNodo.Id}", nuevoNodo);
});

app.Run();

// 2. Modelado del Recurso
public class NodoElemento
{
    public int Id { get; set; }
    public string Valor { get; set; } = string.Empty;
}