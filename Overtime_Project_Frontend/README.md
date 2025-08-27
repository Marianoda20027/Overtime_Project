# Documento de configuración del Frontend


  ### Herramientas Recomendadas

  - IDEs sugeridos y sus configuraciones óptimas.
  - Plugins esenciales para mejorar la productividad y calidad del código.
  - Guías de configuraciones de ambientes.

**Herramientas y plugins**

- IDE sugerido: VSCode
- Extensiones: ESLint, Prettier.

### Pasos para la configuración del ambiente de Frontend

---

## **Paso 1: Descargar e Instalar Node.js**

Accede al sitio web oficial de Node.js en la siguiente [página](https://nodejs.org/), donde podrás encontrar la última versión LTS (Long Term Support).

- **Selecciona la versión recomendada:** Para proyectos de producción, se recomienda elegir la versión LTS indicada en el sitio web.
- **Instalación:** Después de descargar el instalador, abre el archivo y sigue las instrucciones en pantalla.
- **Verificación de la instalación:** Abre una terminal o símbolo del sistema y ejecuta los siguientes comandos para confirmar que Node.js y npm (Node Package Manager) se han instalado correctamente:
   ```bash
   node -v
   npm -v
   ```

   Estos comandos devolverán los números de versión de Node.js y npm si la instalación fue exitosa.

---

### **Paso 2: Instalación de React App utilizando Vite**

- **Crear directorio del proyecto:** Entra en tu terminal y crea un directorio para el proyecto con el siguiente comando:
   ```bash
   mkdir Overtime_Project_Frontend
   cd Overtime_Project_Frontend
   ```

- **Crear proyecto con Vite:** Ejecuta el siguiente comando para crear un nuevo proyecto frontend utilizando Vite:
   ```bash
   npm init vite@latest
   ```

   Durante el proceso de configuración, se te pedirá que confirmes si deseas proceder con la instalación del paquete `create-vite@5.5.5`. Responde con "y".

- **Selecciona opciones:** Se te pedirá que ingreses algunos detalles:
   - Nombre del proyecto: `tiquetes`
   - Framework o librería: **React**
   - Lenguaje: **TypeScript + SWC**

- **Instalación de dependencias:** Una vez creado el proyecto, ingresa al directorio del proyecto y ejecuta los siguientes comandos:
   ```bash
   cd Overtime_Project_Frontend
   npm i
   npm run dev
   ```

   El comando `npm run dev` debe iniciar el servidor de desarrollo y mostrar un sitio web en la dirección local, que puede tener un puerto diferente según el dispositivo.

---

### **Paso 3: Configuración de ESLint**

- **Instalar ESLint:** Ejecuta el siguiente comando en el directorio del proyecto:
   ```bash
   npm init @eslint/config@latest
   ```

- **Configuración en el asistente:** Selecciona las siguientes opciones:
   - Objetivo: **To check syntax, find problems, and enforce code style.**
   - Formato de módulos: **JavaScript modules (import/export)**
   - Framework: **React**
   - Uso de TypeScript: **Yes**
   - Entorno: **Browser**
   - Guía de estilos: **Standard** (https://github.com/standard/eslint-config-standard-with-typescript)
   - Formato de archivo de configuración: **JavaScript**

- **Añadir configuraciones a `eslint.config.js`:** Asegúrate de agregar las siguientes configuraciones:
   ```bash
   {
     "settings": {
       "react": {
         "version": "detect"
       }
     },
   },
   {
     "rules": {
       "react/jsx-uses-react": "error",
       "react/jsx-uses-vars": "error"
     }
   }
   ```

- **Verificar configuración:** Verifica que ESLint está configurado correctamente ejecutando el siguiente comando:
   ```bash
   npx eslint src/main.tsx
   ```

---

### **Paso 4: Configuración de Prettier**

- **Instalar Prettier:** Ejecuta el siguiente comando:
   ```bash
   npm install --save-dev --save-exact prettier --legacy-peer-deps
   ```

- **Crear archivo de configuración de Prettier:** En la raíz de tu proyecto, crea un archivo llamado `.prettierrc` y agrega las siguientes configuraciones:
   ```bash
   {
     "semi": true,
     "singleQuote": true,
     "trailingComma": "es5",
     "printWidth": 80,
     "tabWidth": 2
   }
   ```

- **Ignorar archivos específicos:** Crea un archivo `.prettierignore` en la raíz de tu proyecto para especificar los archivos o directorios que deseas que Prettier ignore, como:
   ```
   dist
   package-lock.json
   ```

- **Verificar Prettier:** Verifica que Prettier está funcionando correctamente con el siguiente comando:
   ```bash
   npx prettier src/main.tsx
   ```

---

### **Paso 5: Configuración de Husky**

- **Instalar Husky:** Ejecuta el siguiente comando para instalar Husky:
   ```bash
   npm install husky --save-dev
   npx husky install
   ```

- **Configurar hook de pre-commit:** Agrega la siguiente configuración en `package.json`:
   ```bash
   "scripts": {
     "prepare": "husky install"
   }
   ```

- **Añadir hook de pre-commit:** Inicializa un hook de pre-commit con el siguiente comando:
   ```bash
   npx husky add .husky/pre-commit "npm run lint"
   ```

- **(Opcional) Instalar Commitlint:** Para validar los mensajes de commit, instala Commitlint con el siguiente comando:
   ```bash
   npm install --save-dev @commitlint/{config-conventional,cli}
   ```

- **Crear archivo de configuración de Commitlint:** Crea un archivo `commitlint.config.js` con el siguiente contenido:
   ```bash
   module.exports = { extends: ["@commitlint/config-conventional"] };
   ```

- **Añadir hook de commit:** Agrega el siguiente hook de commit:
   ```bash
   npx husky add .husky/commit-msg 'npx --no -- commitlint --edit "$1"'
   ```

---

### **Paso 6: Instalación de Librerías para UI**

- **Instalar Ant Design:** Para instalar Ant Design, ejecuta:
   ```bash
   npm install antd
   ```

- **Instalar Material UI con React:** Ejecuta los siguientes comandos para instalar Material UI:
   ```bash
   npm install @mui/material @emotion/react @emotion/styled
   npm install @mui/material @mui/styled-engine-sc styled-components
   npm install @fontsource/roboto
   npm install @mui/icons-material
   ```


