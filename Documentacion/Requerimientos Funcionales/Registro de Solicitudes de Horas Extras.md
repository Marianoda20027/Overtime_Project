# Requerimiento Funcional: Registro de Solicitudes de Horas Extra

## Descripción

El sistema debe permitir que los empleados registren solicitudes de horas extra con fecha, cantidad de horas y justificación, pudiendo adjuntar documentos si es necesario.

## Necesidad

Centralizar todas las solicitudes en un sistema único para reducir pérdida de información, retrasos y errores en el flujo de aprobación, y facilitar la trazabilidad.

## Proceso Actual

Actualmente las solicitudes se realizan por correo, Excel o verbalmente, lo que dificulta el seguimiento y aumenta la probabilidad de errores.

## Solución Propuesta

Formulario web con validaciones en frontend (React) que envíe la solicitud al backend (.NET 8) para registro y notificación automática al manager correspondiente. Registro automático en historial/auditoría.

## Documentos de Referencia

- [Política de Horas Extra]  
- [Manual de Gestión de Overtime]

## Casos de Uso Relacionados

- CU-02: Registrar Solicitud de Overtime
- CU-06: Consultar Historial de Solicitudes (historial de solicitudes registradas)
- CU-07: Notificar Cambio de Estado de Solicitud (notificación tras registro)  

## Criterios de Aceptación

- Validar campos obligatorios y rango de horas permitido.  
- Confirmación visual y por correo al enviar solicitud.  
- Registro en historial accesible según rol.

## Referencias

[HTML5 Form Validations](https://developer.mozilla.org)

---

**Documento Preparado Por:** Mariano Durán  
**Fecha:** 2025-08-15
