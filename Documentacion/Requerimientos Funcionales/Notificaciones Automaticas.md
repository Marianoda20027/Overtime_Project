# Requerimiento Funcional: Notificaciones Automáticas

## Descripción

El sistema debe enviar correos automáticos ante cambios de estado en las solicitudes.

## Necesidad

Evitar que empleados y managers tengan que verificar manualmente el estado.

## Proceso Actual

La comunicación del estado se hace manualmente, generando retrasos.

## Solución Propuesta

Integrar SendGrid/SMTP para enviar plantillas HTML automáticas con información relevante.

## Documentos de Referencia

- [Guía de Comunicación Interna]

## Casos de Uso Relacionados

- CU-06: Notificar Cambio de Estado de Solicitud

## Criterios de Aceptación

- Notificación enviada en menos de 1 minuto tras el cambio de estado.

---

**Documento Preparado Por:** Mariano Durán  
**Fecha:** 2025-08-15
