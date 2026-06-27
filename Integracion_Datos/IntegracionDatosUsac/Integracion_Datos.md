# Integracion_Datos.md

## [cite_start]Parte 1: Evaluación Conceptual y Buenas Prácticas [cite: 7]

### [cite_start]Formatos de Intercambio [cite: 9]
| [cite_start]Formato [cite: 10] | [cite_start]Ventajas [cite: 10] | [cite_start]Desventajas [cite: 10] |
|---|---|---|
| [cite_start]**CSV** [cite: 10] | Formato muy ligero y fácil de parsear. Es ideal y rápido para el manejo de datos planos y estrictamente tabulares. | No soporta estructuras jerárquicas o anidadas complejas. Además, los tipos de datos no están definidos explícitamente (todo es texto). |
| [cite_start]**XML** [cite: 10] | Es altamente descriptivo, fuertemente estandarizado y soporta estructuras de datos jerárquicas muy complejas. | Es un formato muy verboso, lo que genera archivos más pesados. Su procesamiento y parseo es significativamente más lento que otros formatos. |

### [cite_start]1. Diferenciación de Procesos [cite: 11]
[cite_start]Utilizando la librería nativa `System.Text.Json`, la diferencia técnica es la siguiente[cite: 11]:
* **Serialización:** Es el proceso mediante el cual el estado de un objeto residente en memoria (como una instancia de una clase en C#) se convierte en una cadena de texto estructurada en formato JSON para que pueda ser transmitida por la red o guardada en un archivo.
* **Deserialización:** Es el proceso exactamente inverso. Toma un flujo de datos o cadena JSON entrante y la transforma reconstruyéndola en un objeto fuertemente tipado de C# en la memoria de la aplicación.

### [cite_start]2. El Antipatrón del Rendimiento [cite: 12]
* [cite_start]El error de rendimiento "N+1" ocurre durante la lectura de un archivo masivo al ejecutar una operación individual de inserción a la base de datos por cada registro que se lee[cite: 12]. Esto significa que si hay 10,000 registros, el sistema hará 10,000 viajes de red a la base de datos (N) más la conexión inicial (+1), saturando por completo la red y los recursos del servidor.
* [cite_start]La estrategia de optimización por lotes (Batching) soluciona este problema leyendo los registros y acumulándolos primero en una colección en memoria (una lista)[cite: 12]. Una vez que se alcanza un tamaño de lote adecuado (o se termina de leer el archivo), se realiza una única inserción masiva a la base de datos, reduciendo drásticamente la latencia.

---

## [cite_start]Parte 2: Implementación Práctica en C# [cite: 13]

### [cite_start]Desafío 1: Consumo de Endpoints y Deserialización [cite: 16]
[cite_start]El siguiente código implementa el consumo asíncrono hacia la URL `https://api.usac.edu/v1/alumnos` mediante un método GET[cite: 17]. [cite_start]Se asegura el control de errores con bloque `try-catch`, validando el código de estado con `EnsureSuccessStatusCode()` [cite: 19] [cite_start]y se configura la deserialización para que ignore mayúsculas y minúsculas usando `PropertyNameCaseInsensitive = true`[cite: 20, 21].

```csharp
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class ConectividadAcademica
{
    private readonly HttpClient _httpClient;

    public ConectividadAcademica(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Alumno> ObtenerInformacionAlumnoAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("[https://api.usac.edu/v1/alumnos](https://api.usac.edu/v1/alumnos)");
            response.EnsureSuccessStatusCode();

            var jsonPayload = await response.Content.ReadAsStringAsync();

            var opcionesJson = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<Alumno>(jsonPayload, opcionesJson);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error de conectividad al consumir el API: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Se produjo un error inesperado: {ex.Message}");
            throw;
        }
    }
}