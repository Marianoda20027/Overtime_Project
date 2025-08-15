# **Casos de Uso – Sistema de Gestión de Horas Extra (Overtime)**

---

# Caso de Uso
Título del Caso de Uso
CU-01 – Autenticarse en el sistema

### Descripción
Permite a los usuarios registrados ingresar al sistema y acceder a funcionalidades según su rol.

### Actores
Primarios: Colaborador, Supervisor, People Ops, Administrador
Secundarios: Sistema

### Precondiciones
- Usuario registrado y activo.
- Conexión a Internet.

### Postcondiciones
- Usuario autenticado y con acceso a su panel según rol.

### Flujo Principal
1. Usuario ingresa credenciales.
2. Sistema valida credenciales.
3. Usuario es redirigido a su panel correspondiente.

### Flujos Alternativos
FA-01: Recuperar contraseña
1. Usuario solicita restablecimiento de contraseña.
2. Sistema envía correo con enlace de recuperación.
3. Usuario restablece contraseña.

### Prototipos
//: # TODO Agregar prototipos de login

### Requerimientos Especiales
- Autenticación mediante JWT.
- Protección de datos sensibles.

### Escenarios de Prueba
Entrada	Salida Esperada
Credenciales válidas	Acceso concedido
Credenciales inválidas	Mensaje de error y registro de intento fallido

Documento Preparado Por: Mariano Durán
Fecha: 2025-08-15

---

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
Entrada	Salida Esperada
Formulario completo	Solicitud registrada correctamente
Formulario incompleto	Mensaje de error

Documento Preparado Por: Jaziel Rojas
Fecha: 2025-08-15

---

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
Entrada	Salida Esperada
Solicitud pendiente	Aprobada y notificación enviada
Solicitud pendiente	Rechazada y notificación enviada

Documento Preparado Por: Mariano Durán
Fecha: 2025-08-15

---

# Caso de Uso
Título del Caso de Uso
CU-04 – Editar o cancelar solicitud

### Descripción
Permite a un colaborador modificar o anular una solicitud antes de su aprobación o rechazo.

### Actores
Primarios: Colaborador
Secundarios: Sistema

### Precondiciones
- Solicitud en estado pendiente.
- Usuario propietario de la solicitud.

### Postcondiciones
- Solicitud modificada o cancelada correctamente.

### Flujo Principal
1. Usuario accede a historial de solicitudes.
2. Selecciona solicitud a modificar o cancelar.
3. Realiza cambios o confirma cancelación.
4. Sistema actualiza datos y registra la acción.

### Flujos Alternativos
FA-01: Revertir cambios
1. Usuario deshace modificaciones antes de confirmar.
2. Sistema mantiene estado anterior.

### Prototipos
//: # TODO Agregar prototipos de edición

### Requerimientos Especiales
- Control de acceso para editar solo solicitudes propias.

### Escenarios de Prueba
Entrada	Salida Esperada
Solicitud pendiente modificada	Actualización reflejada en historial
Solicitud pendiente cancelada	Solicitud eliminada del listado activo

Documento Preparado Por: Jaziel Rojas
Fecha: 2025-08-15

---

# Caso de Uso
Título del Caso de Uso
CU-05 – Generar reporte mensual de horas extra

### Descripción
Permite a People Ops generar reportes consolidados por departamento y período.

### Actores
Primarios: People Ops
Secundarios: Sistema

### Precondiciones
- Usuario autenticado con rol autorizado.
- Existencia de solicitudes aprobadas.

### Postcondiciones
- Reporte generado y disponible para descarga/exportación.

### Flujo Principal
1. Usuario accede a módulo de reportes.
2. Selecciona rango de fechas y filtros opcionales.
3. Sistema genera reporte consolidado.
4. Usuario descarga en PDF o Excel.

### Flujos Alternativos
FA-01: Programar envío automático
1. Usuario programa envío recurrente.
2. Sistema envía reporte automáticamente al correo designado.

### Prototipos
//: # TODO Agregar prototipos de reporte

### Requerimientos Especiales
- Exportación compatible con PDF y Excel.

### Escenarios de Prueba
Entrada	Salida Esperada
Periodo con datos	Reporte generado correctamente
Periodo sin datos	Reporte vacío con mensaje aclaratorio

Documento Preparado Por: Mariano Durán
Fecha: 2025-08-15

---

# Caso de Uso
Título del Caso de Uso
CU-06 – Consultar historial de solicitudes

### Descripción
Permite a usuarios autorizados revisar todas las acciones realizadas sobre solicitudes para auditoría y trazabilidad.

### Actores
Primarios: People Ops, Supervisor, Administrador
Secundarios: Sistema

### Precondiciones
- Usuario autenticado con rol autorizado.
- Existencia de registros en el sistema.

### Postcondiciones
- Historial completo mostrado y exportable.

### Flujo Principal
1. Usuario accede al módulo “Historial de Solicitudes”.
2. Aplica filtros por fecha, empleado o estado.
3. Sistema consulta registros históricos.
4. Sistema despliega resultados detallados.
5. Usuario puede exportar historial a PDF/Excel.

### Flujos Alternativos
FA-01: No existen registros para filtro
1. Usuario aplica filtros.
2. Sistema no encuentra registros.
3. Muestra mensaje “No se encontraron resultados”.

### Prototipos
//: # TODO Agregar prototipos de historial

### Requerimientos Especiales
- Acceso solo a roles autorizados.
- Registro inmutable con sello de tiempo.

### Escenarios de Prueba
Entrada	Salida Esperada
Filtro por fechas	Lista de acciones en el rango seleccionado
Filtro por colaborador	Lista de acciones realizadas por ese colaborador

Documento Preparado Por: Jaziel Rojas
Fecha: 2025-08-15
