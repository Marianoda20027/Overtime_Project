# **Base de Datos - Sistema de Gestión de Horas Extra (Overtime)**

## **1. Tablas de la Base de Datos**

### **1.1 Tabla: `users`**
Esta tabla almacena información sobre los usuarios (empleados, managers, etc.) del sistema.

| Atributo       | Tipo de Dato   | Descripción                                         |
|----------------|----------------|-----------------------------------------------------|
| `user_id`      | UUID (PK)      | Identificador único del usuario.                   |
| `email`        | VARCHAR(255)   | Correo electrónico del usuario (único).            |
| `password_hash`| VARCHAR(255)   | Contraseña encriptada del usuario.                 |
| `first_name`   | VARCHAR(100)   | Nombre del usuario.                                |
| `last_name`    | VARCHAR(100)   | Apellido del usuario.                              |
| `address`      | VARCHAR(255)   | Dirección física del usuario.                      |
| `phone`        | VARCHAR(50)    | Teléfono del usuario.                              |
| `role`         | VARCHAR(20)    | Rol asignado al usuario: 'admin', 'employee', 'manager', etc. |
| `is_active`    | BOOLEAN        | Estado de la cuenta (activo/inactivo).              |
| `salary`       | DECIMAL(10,2)  | Salario del usuario, utilizado para calcular el costo de las horas extra. |

### **1.2 Tabla: `overtime_requests`**
Esta tabla almacena las solicitudes de horas extra realizadas por los usuarios (empleados).

| Atributo        | Tipo de Dato   | Descripción                                            |
|-----------------|----------------|--------------------------------------------------------|
| `overtime_id`   | UUID (PK)      | Identificador único de la solicitud de overtime.       |
| `user_id`       | UUID (FK)      | Clave foránea que hace referencia al usuario (empleado) que realizó la solicitud. |
| `date`          | DATE           | Fecha de la solicitud.                                 |
| `start_time`    | TIME           | Hora de inicio de la solicitud.                        |
| `end_time`      | TIME           | Hora de finalización de la solicitud.                  |
| `cost_center`   | VARCHAR(255)   | Centro de costo (opcional).                            |
| `justification` | TEXT           | Justificación proporcionada por el empleado.          |
| `status`        | VARCHAR(20)    | Estado de la solicitud: 'pending', 'approved', 'rejected'. |
| `created_at`    | DATETIME       | Fecha de creación de la solicitud.                     |
| `updated_at`    | DATETIME       | Fecha de la última actualización de la solicitud.      |
| `cost`          | DECIMAL(10,2)  | Costo de las horas extra calculado como salario * horas. |

### **1.3 Tabla: `overtime_approvals`**
Esta tabla almacena las aprobaciones o rechazos de las solicitudes de overtime realizadas por los managers.

| Atributo           | Tipo de Dato   | Descripción                                             |
|--------------------|----------------|---------------------------------------------------------|
| `approval_id`      | UUID (PK)      | Identificador único de la aprobación/rechazo.           |
| `overtime_id`      | UUID (FK)      | Clave foránea que hace referencia a la solicitud de overtime. |
| `manager_id`       | UUID (FK)      | Clave foránea que hace referencia al manager que aprobó/rechazó. |
| `approved_hours`   | DECIMAL(5,2)   | Horas aprobadas por el manager.                         |
| `approval_date`    | DATETIME       | Fecha en la que se realizó la aprobación.               |
| `status`           | VARCHAR(20)    | Estado de la solicitud: 'approved', 'rejected'.         |
| `comments`         | TEXT           | Comentarios adicionales del manager.                    |
| `rejection_reason` | TEXT (nullable)| Razón del rechazo, solo se llena si la solicitud es rechazada. |

### **1.4 Tabla: `roles`**
Esta tabla define los roles del sistema, como "empleado", "manager", "admin", etc.

| Atributo       | Tipo de Dato   | Descripción                                             |
|----------------|----------------|---------------------------------------------------------|
| `role_id`      | UUID (PK)      | Identificador único del rol.                            |
| `role_name`    | VARCHAR(50)    | Nombre del rol: 'employee', 'manager', 'admin', etc.    |
| `permissions`  | TEXT           | Descripción de los permisos asociados a este rol.       |

### **1.5 Tabla: `notifications`**
Esta tabla almacena las notificaciones enviadas a los usuarios sobre el estado de sus solicitudes de overtime.

| Atributo        | Tipo de Dato   | Descripción                                            |
|-----------------|----------------|--------------------------------------------------------|
| `notification_id`| UUID (PK)     | Identificador único de la notificación.                |
| `user_id`       | UUID (FK)      | Clave foránea que hace referencia al usuario receptor. |
| `message`       | TEXT           | Mensaje de la notificación.                            |
| `date_sent`     | DATETIME       | Fecha y hora en que la notificación fue enviada.       |
| `status`        | VARCHAR(20)    | Estado de la notificación: 'sent', 'failed', 'pending'. |

