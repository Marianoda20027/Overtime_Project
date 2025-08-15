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

Entrada Salida Esperada
Credenciales válidas Acceso concedido
Credenciales inválidas Mensaje de error y registro de intento fallido

- Documento Preparado Por: Mariano Durán
- Fecha: 2025-08-15

---
