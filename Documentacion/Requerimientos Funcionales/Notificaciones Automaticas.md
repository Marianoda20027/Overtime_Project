# Requerimiento Funcional: Notificaciones Automáticas

## Descripción

El sistema debe enviar notificaciones automáticas (correo electrónico y opcionalmente alertas internas) cuando cambie el estado de una solicitud de horas extra, incluyendo creación, aprobación, rechazo o cancelación.

## Necesidad

Evitar que empleados y managers tengan que verificar manualmente el estado de las solicitudes, reduciendo retrasos y errores en la comunicación.

## Proceso Actual

Actualmente la comunicación del estado se realiza manualmente, mediante correos individuales o mensajes internos, generando retrasos y riesgo de pérdida de información.

## Solución Propuesta

Integrar SendGrid o SMTP para enviar correos automáticos con plantillas HTML que indiquen el estado y detalles de la solicitud. También incluir notificaciones dentro del sistema cuando el usuario esté activo en la plataforma.

## Documentos de Referencia

- [Guía de Comunicación Interna]  
- [Política de Notificaciones de la Empresa]

## Casos de Uso Relacionados

- CU-06: Notificar Cambio de Estado de Solicitud  
- CU-03 / CU-04: Aprobar, Rechazar, Editar o Cancelar Solicitud (acciones que disparan la notificación)

## Criterios de Aceptación

- Notificación enviada en menos de 1 minuto tras el cambio de estado.  
- Mensajes claros, con información completa de la solicitud.  
- Solo los usuarios afectados reciben notificación correspondiente.

---

**Documento Preparado Por:** Mariano Durán  
**Fecha:** 2025-08-15
