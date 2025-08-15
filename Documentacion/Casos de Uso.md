# **Casos de Uso – Sistema de Gestión de Horas Extra (Overtime)**

---

## **CU-01 – Autenticarse en el sistema**

**Descripción:** Permite a cualquier usuario con credenciales válidas ingresar al sistema, accediendo a funciones según su rol asignado.  
**Actores:** Colaborador, Supervisor, People Ops, Administrador.  
**Precondiciones:**

- El usuario debe estar registrado y activo.
- Debe contar con conexión a Internet.

**Flujo principal:**

1. El usuario ingresa usuario y contraseña en el formulario de inicio de sesión.
2. El sistema valida credenciales contra la base de datos.
3. Si son correctas, se establece sesión y se redirige al panel correspondiente al rol.

**Flujo alternativo:**

- Si el usuario olvida la contraseña, puede solicitar restablecimiento vía correo electrónico.

**Excepciones:**

- **E01:** Credenciales inválidas → Se muestra mensaje de error y se registra intento fallido.

**Postcondiciones:**

- El usuario queda autenticado y con acceso a sus módulos autorizados.

**RF asociados:** RF-01.

---

## **CU-02 – Registrar solicitud de horas extra**

**Descripción:** Permite a un colaborador crear una solicitud indicando fecha, hora de inicio, hora de fin, justificación y adjuntos opcionales.  
**Actores:** Colaborador (principal), Sistema (secundario).  
**Precondiciones:**

- Usuario autenticado con rol de Colaborador.
- No debe existir otra solicitud para el mismo rango horario.

**Flujo principal:**

1. El colaborador accede al módulo “Solicitudes de Overtime”.
2. Completa el formulario con los campos requeridos.
3. Adjunta documentos justificativos si aplica.
4. Envía la solicitud.
5. El sistema valida disponibilidad horaria y formato de datos.
6. La solicitud queda registrada con estado “Pendiente” y se notifica al supervisor.

**Flujo alternativo:**

- Si faltan datos, el sistema solicita correcciones antes de guardar.

**Excepciones:**

- **E02:** Horas duplicadas con otra solicitud → Se rechaza el registro y se informa al usuario.

**Postcondiciones:**

- Solicitud creada y visible en el historial del usuario.

**RF asociados:** RF-02, RF-03.

---

## **CU-03 – Aprobar o rechazar solicitud**

**Descripción:** Permite al supervisor revisar solicitudes pendientes y tomar una decisión con base en la justificación y adjuntos.  
**Actores:** Supervisor (principal), Sistema (secundario).  
**Precondiciones:**

- Usuario autenticado como Supervisor.
- Existencia de solicitudes en estado “Pendiente” asignadas al supervisor.

**Flujo principal:**

1. El supervisor ingresa a “Solicitudes pendientes”.
2. Selecciona una solicitud para revisar.
3. Visualiza detalles, justificación y adjuntos.
4. Elige aprobar o rechazar.
5. El sistema actualiza el estado y notifica al solicitante.

**Flujo alternativo:**

- El supervisor puede solicitar aclaraciones antes de decidir.

**Excepciones:**

- **E03:** La solicitud fue modificada o cancelada mientras se revisaba → Se bloquea la acción y se informa.

**Postcondiciones:**

- Estado actualizado y reflejado en reportes.

**RF asociados:** RF-04, RF-05.

---

## **CU-04 – Editar o cancelar solicitud**

**Descripción:** Permite a un colaborador modificar o anular una solicitud siempre que no haya sido aprobada o rechazada.  
**Actores:** Colaborador, Sistema.  
**Precondiciones:**

- Solicitud en estado “Pendiente”.
- Usuario autenticado como propietario de la solicitud.

**Flujo principal:**

1. El colaborador accede a su historial de solicitudes.
2. Selecciona la solicitud a modificar o cancelar.
3. Realiza cambios o confirma cancelación.
4. El sistema actualiza los datos y registra la acción.

**Flujo alternativo:**

- El usuario puede revertir cambios antes de confirmar.

**Excepciones:**

- **E04:** La solicitud ya fue aprobada/rechazada → No se permite modificación.

**Postcondiciones:**

- Solicitud modificada o eliminada del registro activo.

**RF asociados:** RF-06, RF-07.

---

## **CU-05 – Generar reporte mensual de horas extra**

**Descripción:** Permite al personal de People Ops generar un reporte consolidado de horas extra aprobadas, clasificadas por departamento y rango de fechas.  
**Actores:** People Ops, Sistema.  
**Precondiciones:**

- Usuario autenticado con rol autorizado.
- Existencia de solicitudes aprobadas en el período seleccionado.

**Flujo principal:**

1. People Ops ingresa a “Reportes”.
2. Selecciona rango de fechas y filtros opcionales.
3. El sistema extrae datos y genera reporte.
4. El usuario descarga en formato PDF o Excel.

**Flujo alternativo:**

- Se puede programar el envío automático por correo electrónico.

**Excepciones:**

- **E05:** No hay datos en el período → Se genera reporte vacío con mensaje aclaratorio.

**Postcondiciones:**

- Reporte disponible para consulta y archivo.

**RF asociados:** RF-08.

---

## **CU-06 – Configurar parámetros del sistema**

**Descripción:** Permite al administrador ajustar parámetros como políticas de aprobación, límites de horas, roles y notificaciones.  
**Actores:** Administrador, Sistema.  
**Precondiciones:**

- Usuario autenticado como Administrador.

**Flujo principal:**

1. El administrador accede al módulo “Configuraciones”.
2. Modifica parámetros deseados.
3. Guarda cambios.
4. El sistema aplica nuevas reglas en solicitudes futuras.

**Flujo alternativo:**

- El administrador puede exportar configuraciones previas como respaldo.

**Excepciones:**

- **E06:** Error en validación de valores ingresados → Se muestra mensaje y no se guardan cambios.

**Postcondiciones:**

- Configuración actualizada y activa.

**RF asociados:** RF-09.
