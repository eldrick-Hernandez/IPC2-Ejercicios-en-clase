# Actividad: Balanceo Compuesto en Árboles AVL y Web APIs

**Estudiante:** Eldrick Aldair Hernández Bautista  
**Carné:** 202403488  

---

## 1. Análisis Teórico

### El límite de las rotaciones simples
Las rotaciones simples (LL, RR) son insuficientes cuando se insertan secuencias cruzadas (por ejemplo, 30, 10, 20). En estos casos, el árbol adopta una forma de "zig-zag". Una rotación simple solo inclina el árbol hacia el otro lado sin corregir la altura del subárbol central. Para solucionar esto, se aplica una Rotación Doble Izquierda-Derecha (RID), la cual se dispara cuando el Factor de Equilibrio (FE) del nodo padre es -2 y el de su hijo izquierdo tiene el signo opuesto (+1).

### Principio DRY en balanceo compuesto
Desde la perspectiva de ingeniería de software, la implementación de operaciones compuestas (RID, RDI) debe reutilizar los métodos de las rotaciones simples. Esto obedece al principio DRY (*Don't Repeat Yourself*), minimizando la reasignación de punteros desde cero y reduciendo drásticamente la probabilidad de errores en la manipulación de la estructura.

### Arquitectura Web (HTTP)
El proyecto expone la estructura del árbol a través de una API. Se utiliza el método `GET` para consultar el estado actual del árbol, ya que es una operación de solo lectura. Para las inserciones, se emplea el método `POST`, diseñado específicamente para mutar el estado del servidor agregando nuevos elementos.

---

## 2. Implementación y Pruebas

Para validar la lógica implementada, se estructuró el siguiente archivo de pruebas. Este script documenta el flujo de peticiones necesario para evidenciar el balanceo dinámico:

```http
### Paso A: GET (Verificar estado inicial Zig-Zag)
GET http://localhost:5148/api/arbol

### Paso B: POST (Insertar 20 para disparar la rotación RID)
POST http://localhost:5148/api/arbol/insertar
Content-Type: application/json

{
  "id": 20,
  "etiqueta": "Nieto Derecho"
}

### Paso C: GET Final (Verificar que se balanceó el árbol)
GET http://localhost:5148/api/arbol