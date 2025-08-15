# Plan de Proyecto – Automatización del Proceso de Overtime

## 1. Introducción

Este documento describe el plan de trabajo para el desarrollo de un sistema web automatizado para la gestión y aprobación de horas extra (overtime), que busca digitalizar y optimizar el proceso manual actual.

La solución propuesta consta de una aplicación web con backend en .NET 8 y frontend en React, con integración de notificaciones, dashboards y despliegue en nube.

---

## 2. Objetivos del Proyecto

- Digitalizar el proceso de solicitud y aprobación de horas extra.
- Reducir errores y retrasos mediante automatización y trazabilidad.
- Mejorar la experiencia de los usuarios (empleados, managers, People Ops, Payroll).
- Proveer reportes y métricas en tiempo real.
- Implementar un sistema seguro con roles y permisos.
- Desplegar la solución en un ambiente productivo con CI/CD.

---

## 3. Metodología y Alcance

- Duración: 16 semanas, 20 horas semanales.
- Entregables: Aplicación web responsive, sistema autenticación y roles, flujo aprobación, notificaciones, dashboard, reportes, documentación técnica.
- Tecnologías: .NET 8 API REST, React, MySQL, SendGrid/SMTP, hosting Azure/AWS/OCI.

---

## 4. Cronograma Detallado

| Semana | Objetivo / Módulos                 | Descripción y Horas aproximadas (20h/semana)                                                                                                                                 |
| ------ | ---------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **1**  | Análisis y diseño inicial          | - Requerimientos funcionales y no funcionales (8h)<br>- Diagramas UML: Casos de uso (5h), Diagrama de actividades (3h)<br>- Revisión con stakeholders (4h)                   |
| **2**  | Configuración base y autenticación | - Setup proyecto backend y frontend (4h)<br>- Implementación autenticación JWT, login, roles (8h)<br>- Frontend login/registro con validaciones (6h)<br>- Documentación (2h) |
| **3**  | Gestión usuarios y roles           | - Backend CRUD usuarios y roles (8h)<br>- Frontend panel usuarios y roles (7h)<br>- Validaciones y control de acceso (3h)<br>- Documentación (2h)                            |
| **4**  | CRUD solicitudes overtime          | - Backend modelo y endpoints solicitudes (10h)<br>- Frontend formularios y listados (7h)<br>- Documentación módulo solicitudes (3h)                                          |
| **5**  | Flujo de aprobación básico         | - Endpoints aprobación/rechazo (8h)<br>- Frontend vista aprobación con feedback visual (8h)<br>- Documentación flujo aprobación (4h)                                         |
| **6**  | Notificaciones por email           | - Integración SMTP/SendGrid, plantillas HTML (7h)<br>- Backend disparo notificaciones automáticas (6h)<br>- Frontend mensajes de estado (4h)<br>- Documentación (3h)         |
| **7**  | Dashboard seguimiento              | - Backend APIs estadísticas (8h)<br>- Frontend gráficos con Chart.js/Recharts (8h)<br>- Documentación (4h)                                                                   |
| **8**  | Historial y auditoría              | - Backend registro completo acciones (9h)<br>- Frontend historial detallado (7h)<br>- Documentación (4h)                                                                     |
| **9**  | Validaciones reglas negocio        | - Backend reglas negocio (máximos, solapamientos) (10h)<br>- Frontend validaciones y mensajes (6h)<br>- Documentación (4h)                                                   |
| **10** | Recomendaciones inteligentes       | - Backend algoritmo sugerencias (10h)<br>- Frontend recomendaciones y alertas (6h)<br>- Documentación (4h)                                                                   |
| **11** | Integración calendarios            | - Backend integración Google Calendar y Outlook (9h)<br>- Frontend exportar y vista calendario (7h)<br>- Documentación (4h)                                                  |
| **12** | Reportes avanzados                 | - Backend generación y filtrado reportes (8h)<br>- Exportación a PDF, Excel (7h)<br>- Frontend interfaz reportes (3h)<br>- Documentación (2h)                                |
| **13** | Optimización rendimiento           | - Backend optimización consultas, indexación DB (6h)<br>- Frontend optimización carga y UX (6h)<br>- Documentación (8h)                                                      |
| **14** | Pruebas funcionales y QA           | - Diseño y ejecución de pruebas backend/frontend (12h)<br>- Corrección bugs y mejoras UX (6h)<br>- Documentación pruebas (2h)                                                |
| **15** | Despliegue y CI/CD                 | - Configuración CI/CD backend y frontend (8h)<br>- Despliegue en Azure/AWS (10h)<br>- Documentación despliegue (2h)                                                          |
| **16** | Cierre, demo y entrega final       | - Preparación demo funcional para stakeholders (6h)<br>- Ajustes UI y optimización (8h)<br>- Backup y documentación final (6h)                                               |

---

## 5. Descripción de Funcionalidades Clave

### 5.1 Autenticación y Gestión de Usuarios

- Registro, login y roles con JWT.
- Panel administración usuarios y asignación de roles (empleado, manager, People Ops, Payroll).

### 5.2 Solicitudes Overtime

- Formulario para ingresar solicitud con campos: fecha, horas, justificación.
- Listados personalizados por rol: empleado ve sus solicitudes, manager ve solicitudes de su equipo.

### 5.3 Flujo de Aprobación

- Managers pueden aprobar o rechazar solicitudes con un clic.
- Cambios reflejados en tiempo real y notificaciones automáticas.

### 5.4 Notificaciones Automáticas

- Emails automáticos en cada cambio de estado (solicitud creada, aprobada, rechazada).
- Uso de SMTP o SendGrid con plantillas HTML.

### 5.5 Dashboard y Reportes

- Visualización de métricas clave: solicitudes pendientes, aprobadas, tiempos de respuesta.
- Reportes exportables a PDF, Excel y Google Sheets.

### 5.6 Historial y Auditoría

- Registro completo de cada acción para trazabilidad y auditorías internas.

### 5.7 Reglas de Negocio y Validaciones

- Validación de máximos de horas permitidas por periodo.
- Verificación de solapamiento de horarios.

### 5.8 Integración con Calendarios

- Exportación a Google Calendar y Outlook para fácil seguimiento.

### 5.9 Despliegue y CI/CD

- Pipeline automatizado para pruebas y despliegue en entornos productivos en Azure o AWS.
- Frontend hospedado en Vercel o Azure Static Web Apps.

---

## 6. Documentación

- Diagramas UML: casos de uso, actividad, componentes.
- Documentación API REST con Swagger/OpenAPI.
- Manual de usuario para empleados y managers.
- Documentación técnica para mantenimiento y despliegue.

---

## 7. Conclusión

El proyecto tiene un enfoque profesional, orientado a cubrir todas las etapas: análisis, diseño, desarrollo, pruebas, despliegue y documentación, con un cronograma detallado y realista para una dedicación semanal de 20 horas durante 16 semanas.
