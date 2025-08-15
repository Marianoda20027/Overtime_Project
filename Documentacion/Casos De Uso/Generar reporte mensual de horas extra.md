# Caso de Uso

Título del Caso de Uso
CU-05 – Generar reporte mensual de horas extra

### Descripción

Permite a People Ops generar reportes consolidados por departamento y período.

### Actores

Primarios: People Ops
Secundarios: Sistema

### Precondiciones

- Usuario autenticado con rol autorizado.
- Existencia de solicitudes aprobadas.

### Postcondiciones

- Reporte generado y disponible para descarga/exportación.

### Flujo Principal

1. Usuario accede a módulo de reportes.
2. Selecciona rango de fechas y filtros opcionales.
3. Sistema genera reporte consolidado.
4. Usuario descarga en PDF o Excel.

### Flujos Alternativos

FA-01: Programar envío automático

1. Usuario programa envío recurrente.
2. Sistema envía reporte automáticamente al correo designado.

### Prototipos

//: # TODO Agregar prototipos de reporte

### Requerimientos Especiales

- Exportación compatible con PDF y Excel.

### Escenarios de Prueba

Entrada Salida Esperada
Periodo con datos Reporte generado correctamente
Periodo sin datos Reporte vacío con mensaje aclaratorio

- Documento Preparado Por: Mariano Durán
- Fecha: 2025-08-15

---
