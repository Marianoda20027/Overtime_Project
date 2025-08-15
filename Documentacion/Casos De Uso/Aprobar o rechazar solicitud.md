# Caso de Uso

Título del Caso de Uso
CU-03 – Aprobar o rechazar solicitud

### Descripción

Permite a los managers aprobar o rechazar solicitudes de horas extra con un clic, generando notificación al solicitante.

### Actores

Primarios: Supervisor
Secundarios: Sistema

### Precondiciones

- Usuario autenticado como Supervisor.
- Existencia de solicitudes pendientes.

### Postcondiciones

- Estado de la solicitud actualizado y notificación enviada al empleado.

### Flujo Principal

1. Supervisor ingresa al módulo de solicitudes pendientes.
2. Selecciona solicitud y revisa detalles.
3. Decide aprobar o rechazar.
4. Sistema actualiza estado y envía notificación al solicitante.

### Flujos Alternativos

FA-01: Solicitud con información insuficiente

1. Supervisor solicita aclaraciones.
2. Empleado actualiza la solicitud.
3. Supervisor completa aprobación/rechazo.

### Prototipos

//: # TODO Agregar prototipos de aprobación

### Requerimientos Especiales

- Registro en historial/auditoría de cada acción.

### Escenarios de Prueba

Entrada Salida Esperada
Solicitud pendiente Aprobada y notificación enviada
Solicitud pendiente Rechazada y notificación enviada

- Documento Preparado Por: Mariano Durán
- Fecha: 2025-08-15

---
