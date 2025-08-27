# 🛠 Automatización del Proceso de Overtime

## ❌ Problema Actual

El proceso actual de solicitud y aprobación de horas extra es manual y se maneja completamente por correo electrónico, lo que genera múltiples ineficiencias:

- 📧 Múltiples intercambios de emails entre 4 stakeholders
- ❌ Falta de trazabilidad y seguimiento centralizado
- 🕒 Retrasos en aprobaciones por disponibilidad de correo
- ⚠ Errores humanos en el flujo de información
- 👎 Experiencia deficiente del empleado (sin visibilidad del estatus)

---

## ✅ Solución Propuesta: Sistema Web Automatizado de Gestión de Overtime

Una aplicación que digitaliza y automatiza completamente el flujo de aprobación:

1. ✍ El empleado ingresa la solicitud (fecha, horas, justificación).
2. 📩 Notificación automática al manager para su aprobación.
3. ✔ El manager aprueba/rechaza directamente desde la plataforma.
4. 🔁 Notificaciones automáticas a Payroll y People Ops tras la aprobación.
5. 📊 Dashboard de seguimiento para todos los stakeholders.

---

## 👥 Stakeholders Involucrados

| Rol         | Responsabilidad Actual       | Beneficio de la Solución                       |
| ----------- | ---------------------------- | ---------------------------------------------- |
| Empleado    | Enviar email con solicitud   | Interfaz intuitiva, seguimiento en tiempo real |
| Manager     | Revisar y aprobar por email  | Aprobación con un clic, historial completo     |
| People Ops  | Revisar y reenviar a Payroll | Notificación automática, reporting integrado   |
| Payroll SME | Procesar pago manualmente    | Datos estructurados, reducción de errores      |

---

## 📉 Análisis Costo-Beneficio

**Situación Actual (por solicitud)**

| Actor       | Tiempo invertido |
| ----------- | ---------------- |
| Empleado    | 15 min           |
| Manager     | 10 min           |
| People Ops  | 20 min           |
| Payroll SME | 15 min           |
| **Total**   | **60 min**       |

**Con la Solución Automatizada**

| Actor       | Tiempo estimado |
| ----------- | --------------- |
| Empleado    | 3 min           |
| Manager     | 2 min           |
| People Ops  | 2 min           |
| Payroll SME | 5 min           |
| **Total**   | **12 min**      |

### 🧠 Impacto Proyectado

- ⏱ Reducción de tiempo: 80% (48 minutos por solicitud)
- 💵 Ahorro anual estimado:
  - 600 solicitudes/año
  - 480 horas ahorradas
  - $12,000 USD (a $25/hora)

---

## 🌟 Beneficios Estratégicos

### 🚀 Productividad

- Eliminación de cuellos de botella
- -80% en tiempo administrativo
- Liberación de recursos para tareas de valor

### 👥 Experiencia del Empleado

- Proceso transparente y seguimiento en tiempo real
- Menos frustración por visibilidad limitada
- Interfaz moderna y fácil de usar

### 📈 Gestión y Control

- Reporting automático y métricas en tiempo real
- Historial completo y trazabilidad
- Mejor control de costos de overtime

---

## 📦 Alcance del Proyecto - Estudiantes

### Entregables Principales

1. Aplicación Web responsive
2. Sistema de autenticación y roles
3. Flujo de aprobación automatizado
4. Notificaciones por email
5. Dashboard básico de reporting
6. Base de datos segura

### Tecnologías Sugeridas

- **Frontend:** Blazor o Razor Pages (ASP.NET Core)
- **Backend:** .NET 8 con API RESTful
- **Base de Datos:** My SQL
- **Notificaciones:** SMTP integrado o SendGrid
- **Hosting:** Oracle Cloud Infrastructure (OCI), Azure o AWS (tiers gratuitos o educativos)

**Timeline estimado:** 12–16 semanas

---

## 📊 ROI y Justificación

| Métrica               | Estado Actual | Con Solución | Mejora |
| --------------------- | ------------- | ------------ | ------ |
| Tiempo por solicitud  | 60 min        | 12 min       | -80%   |
| Errores humanos       | 15%           | <2%          | -87%   |
| Tiempo de aprobación  | 2–5 días      | <24 horas    | -75%   |
| Satisfacción empleado | 6/10          | 9/10         | +50%   |

**Conclusión:**  
La inversión se recupera en menos de 3 meses solo con el ahorro administrativo.

---

## 🛤 Próximos Pasos

1. ✅ Aprobación del proyecto por stakeholders
2. 📋 Definición detallada de requerimientos con usuarios finales
3. 🎯 Asignación de estudiantes y mentor técnico
4. 🚀 Kick-off y planificación del desarrollo

---

## 🧩 Funcionalidades Adicionales

### 🗂 Historial detallado y auditoría

- Registro completo de cada acción del proceso
- Quién solicitó, aprobó, rechazó y en qué momento
- Apoyo para auditorías internas y cumplimiento normativo

---

### 🧠 Recomendaciones automáticas

- Sugerencias inteligentes de aprobación o rechazo basadas en:
  - Historial de solicitudes del empleado
  - Reglas de negocio: máximo semanal, acumulados mensuales
  - Verificación automática de superposición de horarios

---

### 📆 Integración con Calendarios

- Exportación a Google Calendar y Outlook
- Visualización de fechas aprobadas en calendario personal

---

### 🛡 Control de políticas por rol y área

- Límites de horas por semana
- Validación de horarios según turno laboral
- Aprobadores definidos por departamento o centro de costo

---

### 📈 Reportes avanzados y exportación

- Dashboards personalizables por fecha, persona, área
- Exportación a PDF, Excel o Google Sheets
- Visualización de métricas clave: tasa de aprobación, tiempo medio de respuesta, acumulado mensual, etc.

---
