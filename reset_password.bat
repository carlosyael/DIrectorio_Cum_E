@echo off
chcp 65001 > nul
echo ========================================================
echo   Restablecimiento de Contraseña de Administrador
echo ========================================================
echo.
set /p NEWPASS="Ingrese la nueva contraseña maestra [por defecto 'admin123']: "
if "%NEWPASS%"=="" set NEWPASS=admin123

if exist "publish\portable\TableroDirectorio.exe" (
    cd publish\portable
    TableroDirectorio.exe --reset-password "%NEWPASS%"
    cd ..\..
) else (
    echo Error: No se encontró publish\portable\TableroDirectorio.exe
)

echo.
echo Proceso finalizado.
pause
