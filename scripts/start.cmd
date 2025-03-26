@echo off
setlocal

REM Получаем директорию, в которой находится текущий скрипт
set scriptDir=%~dp0

REM Устанавливаем путь к директории с .NET проектом
set dotnetProjectDir=%scriptDir%..\Valuator

cd /d "%dotnetProjectDir%"

REM Запускаем .NET приложения на разных портах
start "" "dotnet" run --urls http://localhost:5001 -WorkingDirectory %dotnetProjectDir%
start "" "dotnet" run --urls http://localhost:5002 -WorkingDirectory %dotnetProjectDir%
start "" "dotnet" run --urls http://localhost:5003 -WorkingDirectory %dotnetProjectDir%
start "" "dotnet" run --urls http://localhost:5004 -WorkingDirectory %dotnetProjectDir%
@REM start "Valuator 5001" cmd /k "dotnet run --urls http://localhost:5001" -WorkingDirectory %dotnetProjectDir%
@REM start "Valuator 5002" cmd /k "dotnet run --urls http://localhost:5002" -WorkingDirectory %dotnetProjectDir%
@REM start "Valuator 5003" cmd /k "dotnet run --urls http://localhost:5003" -WorkingDirectory %dotnetProjectDir%
@REM start "Valuator 5004" cmd /k "dotnet run --urls http://localhost:5004" -WorkingDirectory %dotnetProjectDir%

REM Запускаем Docker контейнер
docker start my-nginx

endlocal