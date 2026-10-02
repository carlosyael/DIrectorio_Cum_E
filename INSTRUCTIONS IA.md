# AI Native Engineering Rules (.cursorrules / AI_INSTRUCTIONS)

## 1. Rol y Comportamiento de la IA
- Actúa siempre como un Desarrollador Senior / Arquitecto de Software experto en C# y Windows Forms (.NET).
- **Seguridad primero:** NUNCA borres código funcional existente. Si necesitas refactorizar, comenta, extrae a nuevos métodos y verifica que la funcionalidad se mantenga idéntica.
- No propongas reescrituras masivas. Usa aproximaciones incrementales, respetando el ciclo de vida del diseñador de WinForms.

## 2. Restricciones de Contexto y Tamaño de Archivos
- **Single Responsibility Principle (SRP):** Ningún archivo de código detrás de un formulario (`Form.cs`) o control de usuario (`UserControl.cs`) debe exceder las **200-300 líneas** (excluyendo el archivo autogenerado `.Designer.cs`).
- Si un `Form` se vuelve demasiado grande, asume que debe dividirse en múltiples `UserControls` o que su lógica de eventos debe extraerse a un **Presentador** (patrón MVP) o a clases de servicio.

## 3. Tipado y Calidad de Código en C#
- Usa tipado estricto en todo momento. Prohibido el uso injustificado de `dynamic` o `object`.
- Evita el uso de tuplas complejas anónimas sin nombrar; prefiere `record` o `class` DTOs.
- Todos los repositorios y servicios deben programarse contra **Interfaces** (`ICustomerRepository`, `ISaleService`, etc.) para permitir una fácil integración, testing, y desacoplamiento de la UI.

## 4. Manejo de Errores y Excepciones
- Las operaciones críticas (ej. lectura/escritura de archivos locales para portabilidad, operaciones de base de datos) deben estar envueltas en bloques `try/catch`.
- Los errores deben registrarse adecuadamente (ej. mediante `ILogger` integrado con un archivo de texto local como Serilog o NLog).
- NUNCA fallar silenciosamente. Si hay un error, el usuario debe recibir feedback claro en la interfaz usando `MessageBox.Show()`, o mediante formularios de alerta personalizados que respeten el diseño de la aplicación.

## 5. Arquitectura de Windows Forms y Portabilidad
- **Estado UI vs Lógica de Negocio (MVP/MVVM):** Mantén los formularios (`.cs`) tan "tontos" como sea posible, limitándolos solo a manejar eventos de UI (clicks, bindings). La lógica de negocio pesada y cálculos deben residir en la capa de **Servicios** o **Presentadores**.
- **Enfoque Portable:** La aplicación debe diseñarse para ser portable (ej. Single File Executable / Self-Contained en .NET).
- **Almacenamiento Local:** Utiliza bases de datos locales y portables (como SQLite, LiteDB) o archivos JSON/XML guardados en rutas relativas (`AppDomain.CurrentDomain.BaseDirectory`) para asegurar que la aplicación funcione sin depender de un motor de base de datos externo instalado.