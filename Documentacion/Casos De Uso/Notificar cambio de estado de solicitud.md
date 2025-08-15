# Caso de Uso

Título del Caso de Uso
CU-07 – Notificar Cambio de Estado de Solicitud

### Descripción

Permite que el sistema envíe notificaciones automáticas por correo electrónico a empleados y supervisores cada vez que se actualiza el estado de una solicitud de horas extra.

### Actores

Primarios: Sistema  
Secundarios: Colaborador, Supervisor

### Precondiciones

- Solicitud registrada en el sistema.
- Usuario autenticado (receptor de notificación).
- Configuración de correo habilitada y funcional.

### Postcondiciones

- Notificación enviada y registrada en historial/auditoría.
- Usuario recibe mensaje con información del cambio de estado.

### Flujo Principal

1. Supervisor aprueba o rechaza solicitud (CU-03).
2. Sistema detecta cambio de estado.
3. Sistema genera mensaje de notificación con detalle de la solicitud.
4. Sistema envía correo al empleado y opcionalmente al supervisor.
5. Sistema registra acción en historial/auditoría.

### Flujos Alternativos

FA-01: Fallo en envío de notificación

1. Sistema intenta enviar correo pero falla.
2. Se registra el error en log.
3. Sistema reintenta envío o notifica al administrador.

### Prototipos

//: # TODO Agregar prototipos de correo de notificación

### Requerimientos Especiales

- Notificación enviada en menos de 1 minuto tras el cambio de estado.
- Plantillas HTML personalizables según tipo de usuario.

### Escenarios de Prueba

Entrada Salida Esperada
Solicitud aprobada Correo enviado al empleado con estado “Aprobada”
Solicitud rechazada Correo enviado al empleado con estado “Rechazada”

- Documento Preparado Por: Mariano Durán
- Fecha: 2025-08-15
