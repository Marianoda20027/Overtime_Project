# Caso de Uso

Título del Caso de Uso
CU-04 – Editar o cancelar solicitud

### Descripción

Permite a un colaborador modificar o anular una solicitud antes de su aprobación o rechazo.

### Actores

Primarios: Colaborador
Secundarios: Sistema

### Precondiciones

- Solicitud en estado pendiente.
- Usuario propietario de la solicitud.

### Postcondiciones

- Solicitud modificada o cancelada correctamente.

### Flujo Principal

1. Usuario accede a historial de solicitudes.
2. Selecciona solicitud a modificar o cancelar.
3. Realiza cambios o confirma cancelación.
4. Sistema actualiza datos y registra la acción.

### Flujos Alternativos

FA-01: Revertir cambios

1. Usuario deshace modificaciones antes de confirmar.
2. Sistema mantiene estado anterior.

### Prototipos

//: # TODO Agregar prototipos de edición

### Requerimientos Especiales

- Control de acceso para editar solo solicitudes propias.

### Escenarios de Prueba

Entrada Salida Esperada
Solicitud pendiente modificada Actualización reflejada en historial
Solicitud pendiente cancelada Solicitud eliminada del listado activo

- Documento Preparado Por: Jaziel Rojas
- Fecha: 2025-08-15

---
