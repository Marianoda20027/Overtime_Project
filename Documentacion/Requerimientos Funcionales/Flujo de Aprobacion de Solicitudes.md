# Requerimiento Funcional: Flujo de Aprobación de Solicitudes

## Descripción

Los managers deben poder aprobar o rechazar solicitudes de horas extra con un clic, enviando retroalimentación al empleado y asegurando trazabilidad de la acción.

## Necesidad

Reducir tiempos de aprobación y mejorar la comunicación del estado de la solicitud, evitando pérdidas o retrasos que afectan la planificación de horas extra.

## Proceso Actual

La aprobación depende de mensajes de correo o comunicación verbal, lo que puede provocar retrasos, duplicidad o falta de registro histórico.

## Solución Propuesta

Implementar un módulo de aprobación en el frontend que permita aprobar o rechazar solicitudes con un clic, con endpoints en el backend para actualizar el estado. Las acciones generan notificaciones automáticas y se registran en el historial para auditoría.

## Documentos de Referencia

- [Manual de Gestión de Overtime]  
- [Política de Aprobaciones Internas]

## Casos de Uso Relacionados

- CU-03: Aprobar o Rechazar Solicitud  
- CU-04: Editar o Cancelar Solicitud (como alternativa si se requiere ajuste antes de aprobar)

## Criterios de Aceptación

- Estado actualizado en tiempo real tras la acción.  
- Notificación enviada al solicitante inmediatamente.  
- Registro en historial/auditoría inmutable y accesible solo a roles autorizados.

---

**Documento Preparado Por:** Jaziel Rojas  
**Fecha:** 2025-08-15
