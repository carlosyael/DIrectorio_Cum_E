# 🛡️ Tablero Directorio Portable

**Gerencia de Cumplimiento Ético** — Directorio de Tableros e Informaciones

Aplicación de escritorio portable desarrollada en .NET 9 (Windows Forms) que funciona como un directorio centralizado tipo dashboard para gestionar y acceder a tableros de Power BI, archivos Excel y páginas web.

## ✨ Características

- 📊 **Dashboard de tarjetas** con diseño oscuro moderno (glassmorphism)
- 🔍 **Búsqueda en tiempo real** para filtrar tableros
- ➕ **CRUD completo** — Crear, leer, editar y eliminar accesos directos
- 🔐 **Configuración protegida** por contraseña maestra
- 💾 **Base de datos SQLite** portable (archivo `tablero.db`)
- 📦 **Ejecutable portable** — Sin instalación, funciona desde USB o red

## 🚀 Inicio Rápido

### Opción 1: Descargar Release
1. Ve a la sección [Releases](../../releases)
2. Descarga el archivo `.zip` más reciente
3. Extrae y ejecuta `TableroDirectorio.exe`

### Opción 2: Compilar desde código
```bash
# Clonar el repositorio
git clone <url-del-repo>

# Compilar en modo Release portable
dotnet publish src/TableroDirectorio/TableroDirectorio.csproj \
  -c Release -r win-x64 --self-contained \
  -p:PublishSingleFile=true \
  -p:EnableCompressionInSingleFile=true \
  -o publish/portable
```

## 🔑 Contraseña por Defecto

| Campo | Valor |
|-------|-------|
| Contraseña maestra | `admin123` |

> ⚠️ **Importante:** Cambia la contraseña en ⚙️ Configuración después del primer acceso.

## 🏗️ Stack Tecnológico

| Componente | Tecnología |
|-----------|------------|
| Framework | .NET 9 (Windows Forms) |
| Base de datos | SQLite |
| ORM | Dapper |
| Logging | Serilog |
| CI/CD | GitHub Actions |

## 📂 Estructura del Proyecto

```
src/TableroDirectorio/
├── Data/               # Capa de datos (SQLite, repositorios)
├── Models/             # Modelos de dominio
├── Services/           # Lógica de negocio
├── Presenters/         # Patrón MVP (presentadores)
├── UI/                 # Interfaz de usuario
│   ├── Controls/       # Controles personalizados (tarjetas, buscador)
│   ├── Dialogs/        # Diálogos (editor, configuración, contraseña)
│   └── Themes/         # Tema oscuro
└── Helpers/            # Utilidades (hash, rutas)
```

## 📋 Tipos de Recursos Soportados

| Tipo | Extensión | Aplicación |
|------|-----------|------------|
| Power BI | `.pbix` | Power BI Desktop |
| Excel | `.xlsx`, `.xls` | Microsoft Excel |
| Página Web | URL | Navegador predeterminado |
| Carpeta | Ruta | Explorador de Windows |

## 🔄 Crear un Release

```bash
git tag v1.0.0
git push origin v1.0.0
```

El workflow de GitHub Actions se encargará de compilar, empaquetar y publicar el release automáticamente.

---

**Dirección de Cumplimiento Ético © 2026**
