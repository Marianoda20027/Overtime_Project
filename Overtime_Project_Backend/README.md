# Documento de configuración del Backend

### Pasos para la configuración del ambiente de Frontend

---

## **Paso 1: Descargar e Instalar SDK**

Accede al sitio web oficial de .NET en la siguiente [página](https://dotnet.microsoft.com/es-es/download/visual-studio-sdks), donde podrás encontrar la última versión LTS (Long Term Support).

- **Selecciona la versión recomendada:** Para proyectos de producción, se recomienda elegir la versión LTS indicada en el sitio web.
- **Instalación:** Después de descargar el instalador, abre el archivo y sigue las instrucciones en pantalla.
- **Verificación de la instalación:** Abre una terminal o símbolo del sistema y ejecuta los siguientes comandos para confirmar que .NET se ha instalado correctamente:
   ```bash
   dotnet --version
   ```

   Este comando devolverá la versión instalada del SDK instalado

---

### **Paso 2: Crear el proyecto**

- **Crear directorio del proyecto:** Entra en tu terminal y crea un directorio para el proyecto con el siguiente comando:
   ```bash
   mkdir Overtime_Project_Backend
   cd Overtime_Project_Backend
   ```

- **Crear proyecto con .NET:** Ejecuta el siguiente comando para crear un nuevo proyecto backend utilizando .NET:
   ```bash
   dotnet new webapi -n Overtime_Project_Backend
   cd Overtime_Project_Backend
   ```

   De esta manera se crea un proyecto base con controladores base de ejemplo así como Swagger/OpenAPI ya configurado para probar endpoints desde una interfaz gráfica.

### **Paso 3: Ejecutar el proyecto**

Ejecuta en la terminal el siguiente comando:

```bash
   dotnet run
   ```

Este proyecto se ejecutará en la dirección: http://localhost:5100/

Si se desea acceder a la interfáz gráfica disponible por Swagger se accede a: http://localhost:5100/swagger/index.html




