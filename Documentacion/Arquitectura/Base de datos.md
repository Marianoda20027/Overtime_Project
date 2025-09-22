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

## **2. Migraciones con Entity Framework Core**

### **2.1 Crear la migración inicial**
En la carpeta del proyecto API, ejecutar:

```powershell
dotnet ef migrations add InitialCreate
dotnet ef database update
````

Esto crea la base de datos y las tablas automáticamente.

---

## **3. Usar SQL Server con Docker**

### **3.1 Descargar y correr contenedor**

En lugar de instalar SQL Server manualmente, usamos Docker:

```powershell
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Your_password123" `
   -p 1433:1433 --name sql-overtime -d mcr.microsoft.com/mssql/server:2022-latest
```

* Usuario: `sa`
* Contraseña: `Your_password123`
* Puerto expuesto: `1433`

### **3.2 Cadena de conexión**

En `appsettings.json` configurar así:

```json
"ConnectionStrings": {
  "Default": "Server=localhost,1433;Database=OvertimeDb;User Id=sa;Password=Your_password123;TrustServerCertificate=True;"
}
```

### **3.3 Aplicar migraciones dentro del contenedor**

Ya con el contenedor arriba, ejecutar:

```powershell
dotnet ef database update
```

Esto creará la base `OvertimeDb` dentro del SQL Server en Docker.

---

## **4. Integración en el proyecto**

En `Program.cs` agregar que las migraciones se apliquen al iniciar la API:

```csharp
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OvertimeContext>();
    db.Database.Migrate(); // crea la base y aplica migraciones pendientes
}
