@echo off
REM Принудительно завершаем все процессы с именем "valuator.exe"
taskkill /f /im valuator.exe

REM Останавливаем Docker контейнер с именем "my-nginx"
docker stop my-nginx