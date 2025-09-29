# **Base de Datos - Sistema de Gestión de Horas Extra (Overtime)**

## **1. Tablas de la Base de Datos**

*(Generadas automáticamente con Entity Framework Core a partir de las entidades del proyecto, se muestran aquí como referencia.)*

### **1.1 Tabla: `users`**
Guarda a los usuarios del sistema (empleados, managers, admin, etc.).

| Campo           | Tipo            | Descripción |
|-----------------|-----------------|-------------|
| UserId          | UUID (PK)       | Identificador único |
| Email           | NVARCHAR(255)   | Correo electrónico único |
| Password_Hash   | NVARCHAR(255)   | Contraseña encriptada |
| First_Name      | NVARCHAR(100)   | Nombre |
| Last_Name       | NVARCHAR(100)   | Apellido |
| Address         | NVARCHAR(255)   | Dirección |
| Phone           | NVARCHAR(50)    | Teléfono |
| Role            | NVARCHAR(20)    | Rol asignado |
| IsActive        | BIT             | Estado de la cuenta |
| Salary          | DECIMAL(10,2)   | Salario |

### **1.2 Tabla: `overtime_requests`**
Solicitudes de horas extra realizadas por los empleados.

| Campo           | Tipo            | Descripción |
|-----------------|-----------------|-------------|
| OvertimeId      | UUID (PK)       | Identificador de la solicitud |
| UserId (FK)     | UUID            | Usuario que solicita |
| Date            | DATE            | Fecha de la solicitud |
| StartTime       | TIME            | Hora inicio |
| EndTime         | TIME            | Hora fin |
| CostCenter      | NVARCHAR(255)   | Centro de costo |
| Justification   | TEXT            | Justificación |
| Status          | NVARCHAR(20)    | Estado ('pending','approved','rejected') |
| CreatedAt       | DATETIME2       | Creación |
| UpdatedAt       | DATETIME2       | Última actualización |
| Cost            | DECIMAL(10,2)   | Costo calculado |

### **1.3 Tabla: `overtime_approvals`**
Aprobaciones/rechazos de solicitudes.

| Campo            | Tipo            | Descripción |
|------------------|-----------------|-------------|
| ApprovalId       | UUID (PK)       | Identificador |
| OvertimeId (FK)  | UUID            | Solicitud asociada |
| ManagerId (FK)   | UUID            | Manager que aprueba/rechaza |
| ApprovedHours    | DECIMAL(5,2)    | Horas aprobadas |
| ApprovalDate     | DATETIME2       | Fecha |
| Status           | NVARCHAR(20)    | 'approved' o 'rejected' |
| Comments         | TEXT            | Comentarios |
| RejectionReason  | TEXT            | Razón rechazo |

### **1.4 Tabla: `roles`**
Roles del sistema.

| Campo      | Tipo          | Descripción |
|------------|---------------|-------------|
| RoleId     | UUID (PK)     | Identificador único |
| RoleName   | NVARCHAR(50)  | Nombre del rol |
| Permissions| TEXT          | Permisos |

### **1.5 Tabla: `notifications`**
Notificaciones para los usuarios.

| Campo           | Tipo          | Descripción |
|-----------------|---------------|-------------|
| NotificationId  | UUID (PK)     | Identificador único |
| UserId (FK)     | UUID          | Usuario que recibe |
| Message         | TEXT          | Mensaje |
| DateSent        | DATETIME2     | Fecha de envío |
| Status          | NVARCHAR(20)  | Estado ('sent','failed','pending') |

---

# Guía de Migraciones y Conexión a la Base de Datos con SQL Server Management Studio (SSMS)

## *1. Instalación de Dependencias y SQL Server en Docker*

### Paso 1: *Instalar Docker*
1. *Docker* es esencial para ejecutar contenedores con SQL Server de manera sencilla. Si aún no tienes Docker instalado, puedes descargarlo e instalarlo desde su [página oficial](https://www.docker.com/get-started).
   
### Paso 2: *Ejecutar SQL Server en Docker*
1. Usamos un contenedor de *SQL Server* para tener una base de datos en Docker, que nos permite hacer las migraciones y trabajar con SQL Server. Ejecuta el siguiente comando en la terminal:
   bash
   docker run -e 'ACCEPT_EULA=Y' -e 'MSSQL_SA_PASSWORD=Your_password123' -p 1433:1433 --name sql-overtime -d mcr.microsoft.com/mssql/server:2022-latest
`

* *ACCEPT_EULA=Y*: Acepta los términos de la licencia.
* *MSSQL_SA_PASSWORD=Your_password123*: Establece la contraseña del superusuario sa.
* *-p 1433:1433*: Mapea el puerto 1433 de SQL Server al puerto 1433 de tu máquina.
* *--name sql-overtime*: Le asigna un nombre al contenedor.
* **-d mcr.microsoft.com/mssql/server:2022-latest**: Inicia SQL Server en un contenedor Docker.

2. Para verificar que el contenedor está corriendo, ejecuta:

   bash
   docker ps
   

---

## *2. Realizar Migraciones en el Proyecto con Entity Framework (EF Core)*

### Paso 1: *Configurar Entity Framework en el Proyecto*

1. Asegúrate de tener instalado el paquete de *Entity Framework Core* en tu proyecto de backend.

   bash
   dotnet add package Microsoft.EntityFrameworkCore.SqlServer
   dotnet add package Microsoft.EntityFrameworkCore.Tools
   

### Paso 2: *Agregar la Primera Migración*

1. En tu terminal, dentro del directorio del proyecto, agrega la migración inicial con:

   bash
   dotnet ef migrations add InitialMigration
   

   Esto generará un archivo con las instrucciones necesarias para crear las tablas en la base de datos de SQL Server.

### Paso 3: *Aplicar la Migración a la Base de Datos*

1. Para aplicar la migración y crear las tablas en SQL Server, usa el siguiente comando:

   bash
   dotnet ef database update
   

---

## *3. Conectar con SQL Server usando SQL Server Management Studio (SSMS)*

### Paso 1: *Instalar SQL Server Management Studio (SSMS)*

1. *SQL Server Management Studio (SSMS)* es una herramienta gráfica que facilita la gestión de SQL Server. Si no tienes *SSMS*, puedes descargarla desde [aquí](https://aka.ms/ssmsfullsetup).

### Paso 2: *Conectar SSMS a tu SQL Server en Docker*

1. Abre *SQL Server Management Studio (SSMS)*.
2. En la ventana de *Conexión al Servidor*, llena los siguientes campos:

   * *Servidor:* localhost,1433
   * *Autenticación:* *Autenticación de SQL Server*
   * *Nombre de usuario:* sa
   * *Contraseña:* Your_password123
3. Haz clic en *Conectar*.

### Paso 3: *Ver las Bases de Datos*

1. Una vez conectado, en el panel izquierdo, busca la base de datos llamada *OvertimeDb*.
2. Haz clic derecho sobre *OvertimeDb* y selecciona *Nueva consulta* para empezar a ejecutar comandos SQL.

---

## *4. Gestión de Tablas y Datos con SSMS*

### Paso 1: *Ver las Tablas Existentes*

1. Para ver las tablas de la base de datos *OvertimeDb*, ejecuta:

   sql
   SELECT name FROM sys.tables;
   

2. Esto te mostrará una lista de las tablas en tu base de datos.

### Paso 2: *Insertar Datos en la Tabla Users*

1. Para insertar registros en la tabla *Users*, usa un comando SQL similar a este:

   sql
   INSERT INTO dbo.Users (UserId, Email, PasswordHash, Role, IsActive, Salary)
   VALUES 
   ('b241-4d28be0e8499', 'j.perez@arkoselabs.com', 'hashed_password_1', 'Admin', 1, 50000.00),
   ('1-9f37-49a5-b11d-bf6b2f96c64b', 'm.lopez@arkoselabs.com', 'hashed_password_2', 'User', 1, 35000.00);
   

### Paso 3: *Ver los Datos Insertados*

1. Para ver los registros insertados en la tabla *Users*, ejecuta:

   sql
   SELECT * FROM dbo.Users;
   

---

## *5. Solución de Problemas Comunes*

### *Error: Invalid column name 'PasswordHash'*

* Este error indica que la columna PasswordHash no existe en la tabla. Asegúrate de que la tabla Users esté correctamente definida y que PasswordHash esté incluido en la creación de la tabla.

### *Error: Conversion failed when converting from a character string to uniqueidentifier*

* Este error ocurre si intentas insertar un valor que no es un *GUID* válido en una columna que espera un *uniqueidentifier. Asegúrate de que los valores de UserId sean **UUID válidos*.

### *Error: Incorrect syntax near*

* Este error generalmente ocurre por comas o paréntesis mal colocados. Revisa cuidadosamente la sintaxis de los comandos INSERT INTO y asegúrate de que cada valor esté correctamente formateado.

---

## *Conclusión*

Siguiendo estos pasos, ahora tienes un entorno de SQL Server corriendo en un contenedor Docker y puedes gestionarlo cómodamente usando *SQL Server Management Studio (SSMS). Además, con **Entity Framework (EF Core)*, puedes realizar migraciones y gestionar tu base de datos de manera eficiente.

¡Ya está todo listo! Si tienes alguna duda o necesitas más ayuda, no dudes en preguntar.

```

Este resumen cubre:
- La *instalación de Docker y SQL Server* en un contenedor.
- Cómo realizar *migraciones con Entity Framework*.
- Conexión y gestión de *SQL Server* usando *SQL Server Management Studio (SSMS)*.
- Solución de algunos *errores comunes*.
