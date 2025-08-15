# Caso de Uso

Título del Caso de Uso
CU-06 – Consultar historial de solicitudes

### Descripción

Permite a usuarios autorizados revisar todas las acciones realizadas sobre solicitudes para auditoría y trazabilidad.

### Actores

Primarios: People Ops, Supervisor, Administrador
Secundarios: Sistema

### Precondiciones

- Usuario autenticado con rol autorizado.
- Existencia de registros en el sistema.

### Postcondiciones

- Historial completo mostrado y exportable.

### Flujo Principal

1. Usuario accede al módulo “Historial de Solicitudes”.
2. Aplica filtros por fecha, empleado o estado.
3. Sistema consulta registros históricos.
4. Sistema despliega resultados detallados.
5. Usuario puede exportar historial a PDF/Excel.

### Flujos Alternativos

FA-01: No existen registros para filtro

1. Usuario aplica filtros.
2. Sistema no encuentra registros.
3. Muestra mensaje “No se encontraron resultados”.

### Prototipos

//: # TODO Agregar prototipos de historial

### Requerimientos Especiales

- Acceso solo a roles autorizados.
- Registro inmutable con sello de tiempo.

### Escenarios de Prueba

Entrada Salida Esperada
Filtro por fechas Lista de acciones en el rango seleccionado
Filtro por colaborador Lista de acciones realizadas por ese colaborador

- Documento Preparado Por: Jaziel Rojas
- Fecha: 2025-08-15
