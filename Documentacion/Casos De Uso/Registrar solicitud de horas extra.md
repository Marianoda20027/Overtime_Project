# Caso de Uso

Título del Caso de Uso
CU-02 – Registrar solicitud de horas extra

### Descripción

Permite a los empleados registrar solicitudes de overtime indicando fecha, cantidad de horas y justificación.

### Actores

Primarios: Colaborador
Secundarios: Sistema

### Precondiciones

- Usuario autenticado como Colaborador.
- No solapamiento con otras solicitudes existentes.

### Postcondiciones

- Solicitud registrada y visible en historial del usuario.

### Flujo Principal

1. Usuario accede al módulo de solicitudes.
2. Completa formulario con fecha, horas y justificación.
3. Adjunta documentos opcionales.
4. Envía solicitud al sistema.
5. Sistema valida datos y registra solicitud.
6. Notificación enviada al supervisor.

### Flujos Alternativos

FA-01: Datos incompletos

1. Usuario intenta enviar solicitud sin completar todos los campos.
2. Sistema alerta de campos faltantes.
3. Usuario corrige y envía nuevamente.

### Prototipos

//: # TODO Agregar prototipos de formulario

### Requerimientos Especiales

- Validaciones de campos obligatorios y rango de horas.

### Escenarios de Prueba

Entrada Salida Esperada
Formulario completo Solicitud registrada correctamente
Formulario incompleto Mensaje de error

- Documento Preparado Por: Jaziel Rojas
- Fecha: 2025-08-15

---
