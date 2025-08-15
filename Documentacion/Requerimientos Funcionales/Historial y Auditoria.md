# Requerimiento Funcional: Historial y Auditoría

## Descripción

Registrar todas las acciones realizadas en el sistema (creación, modificación, aprobación, rechazo, cancelación de solicitudes) para fines de auditoría y trazabilidad completa.

## Necesidad

Asegurar trazabilidad, cumplir con auditorías internas y externas, y permitir revisiones de comportamiento de usuarios en el sistema.

## Proceso Actual

No existe un registro histórico centralizado; el seguimiento se realiza manualmente o mediante correos electrónicos, dificultando la auditoría y la trazabilidad.

## Solución Propuesta

Implementar un módulo de historial que registre automáticamente todas las acciones relevantes con sello de tiempo y usuario responsable. Solo roles autorizados (Administrador, People Ops) pueden acceder a estos registros.

## Documentos de Referencia

- [Normativa de Auditoría Interna]  
- [Política de Seguridad y Control de Accesos]

## Casos de Uso Relacionados

- CU-06: Consultar Historial de Solicitudes  
- CU-03 / CU-04: Aprobar, Rechazar, Editar o Cancelar Solicitud (acciones que quedan registradas)

## Criterios de Aceptación

- Registro inmutable con sello de tiempo.  
- Acceso restringido a roles autorizados.  
- Inclusión de todos los eventos críticos del sistema (creación, modificación, aprobación, rechazo, cancelación).

---

**Documento Preparado Por:** Mariano Durán  
**Fecha:** 2025-08-15
