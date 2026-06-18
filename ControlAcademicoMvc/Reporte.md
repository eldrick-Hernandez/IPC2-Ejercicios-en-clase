# Reporte de Laboratorio: Arquitectura Multi-Nivel y Patrón MVC

## Parte 1: Fundamentación Teórica y Análisis Crítico

### 1. El Tránsito hacia los Sistemas Distribuidos y Multi-Capa
*   **La Limitación del Monolito Local:** Cuando todo el sistema reside en una sola máquina física, la aplicación no puede escalar fácilmente. Si el tráfico aumenta drásticamente, el único servidor se saturará (cuello de botella) y, si el hardware falla, el sistema entero se cae. Además, no se pueden actualizar partes específicas sin reiniciar toda la aplicación.
*   **Distinción Crítica (Layers vs. Tiers):** 
    *   **Layers (Capas Lógicas):** Es la forma en que se organiza el código dentro del mismo proyecto o espacio de memoria (por ejemplo, separar en carpetas distintas las clases de datos y las de lógica).
    *   **Tiers (Niveles Físicos):** Es la separación en distintas máquinas físicas o servidores reales (por ejemplo, tener un servidor web en un país y el servidor de base de datos en otro).
*   **Responsabilidades en la Arquitectura de 3 Niveles:**
    *   **Nivel 1 (Presentation Tier):** Su misión es mostrar información al usuario y capturar sus interacciones. *Tecnologías comunes:* HTML, CSS, JavaScript, React, Angular.
    *   **Nivel 2 (Application Tier):** Su misión es procesar las reglas de negocio, realizar cálculos y conectar la interfaz con los datos. *Tecnologías comunes:* Servidores con ASP.NET, Node.js, Spring Boot.
    *   **Nivel 3 (Data Tier):** Su misión es almacenar, recuperar y asegurar la persistencia de la información. *Tecnologías comunes:* SQL Server, PostgreSQL, MongoDB.
*   **Seguridad Perimetral:** Exponer el puerto de una base de datos directamente a internet es un error crítico porque permite a cualquier atacante intentar conexiones de fuerza bruta o inyecciones directas. La buena práctica es aislar la base de datos en una Red Privada (VPC) para que únicamente el Nivel 2 (Servidor de Aplicaciones) tenga permiso y acceso para comunicarse con ella.

### 2. Desacoplamiento Lógico con el Patrón MVC
*   **La Crisis del Código Espagueti:** Mezclar bases de datos, matemáticas e interfaces HTML en un solo archivo hace que el código sea ilegible y muy frágil; al cambiar un color de la interfaz podrías romper una consulta SQL. Además, hace imposible realizar pruebas unitarias automatizadas porque no puedes aislar la lógica del diseño.
*   **Separación de Preocupaciones (SoC) en MVC:**
    *   **Modelo:** Representa la estructura de los datos (las entidades) y las reglas de negocio. No debe conocer absolutamente nada sobre cómo se van a mostrar esos datos en pantalla.
    *   **Vista:** Es una entidad pasiva encargada de la interfaz gráfica. Tiene estrictamente prohibido conectarse a la base de datos o realizar cálculos matemáticos complejos; solo dibuja lo que le mandan.
    *   **Controlador:** Es el director de orquesta. Recibe la petición del usuario, solicita información al Modelo y se la envía a la Vista adecuada.
*   **Métricas de Ingeniería:** MVC garantiza **Alta Cohesión** porque cada archivo hace una sola cosa específica muy bien, y **Bajo Acoplamiento** porque puedes modificar una Vista sin tener que reescribir ni tocar el código del Modelo.

---

## Parte 2: Modelado del Ciclo de Vida y Enrutamiento Semántico

### 1. Mapeo Analítico de URLs

| URL Entrante del Cliente | Clase Controladora Buscada por el Framework | Método (Acción) Ejecutado | Parámetro Inyectado |
| :--- | :--- | :--- | :--- |
| `.../ControlAcademico/Login` | ControlAcademicoController | Login | (Ninguno) |
| `.../Estudiante/Historial/20260123` | EstudianteController | Historial | 20260123 |
| `.../Asignacion/Detalle/10` | AsignacionController | Detalle | 10 |
| `.../Home` | HomeController | Index (Asignado por defecto) | (Ninguno Opcional) |

### 2. Diagramación del Flujo Interactivo (Ciclo de Vida HTTP)
1. **Petición:** El usuario hace clic en el navegador, el cual genera una petición HTTP que viaja a través de la red hacia el servidor web.
2. **Enrutamiento y Controlador:** El motor de ASP.NET analiza la URL y dirige la petición al **Controlador** correspondiente.
3. **Interacción con el Modelo:** El Controlador analiza lo solicitado e invoca al **Modelo** para extraer o guardar la información necesaria de la base de datos.
4. **Retorno de Datos:** El Modelo le devuelve la información estructurada y procesada al Controlador.
5. **Renderizado de la Vista:** El Controlador inyecta los datos del Modelo en la **Vista**. La Vista genera el HTML final estático y el servidor responde enviando ese HTML de vuelta al navegador del usuario.

---

## Parte 5: Referencias Bibliográficas
* Facultad de Ingeniería, USAC. (2026). Sesión 11: Modelado Base y Arquitecturas de Despliegue. Evolución de Sistemas Distribuidos, Fundamentos del Modelo Cliente-Servidor y Diseño Físico Multi-Capas (N-Tier). Laboratorio del curso Introducción a la Programación y Computación 2. Guatemala.
* Facultad de Ingeniería, USAC. (2026). Sesión 12: Arquitectura y Componentes del Patrón MVC. Desacoplamiento Lógico de Software, Ciclo de Vida de las Peticiones y Enrutamiento en Aplicaciones Interactivas Modernas. Laboratorio del curso Introducción a la Programación y Computación 2. Guatemala.