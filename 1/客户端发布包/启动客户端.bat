@echo off
chcp 65001 >nul
title 🎒 客户端启动中...

set ROOT=%~dp0..

REM 确保 MySQL 已启动
echo 正在检查数据库...
sc query MySQL80 | find "RUNNING" >nul
if errorlevel 1 (
    net start MySQL80 >nul 2>&1
)
:wait_db
sc query MySQL80 | find "RUNNING" >nul
if errorlevel 1 (
    timeout /t 2 /nobreak >nul
    goto wait_db
)
echo 数据库就绪 ✅

REM 启动后端 API
start /min "后端服务" dotnet run --project "%ROOT%\3\Assess.WebApi"

REM 启动米奈 AI
start /min "米奈AI" dotnet run --project "%ROOT%\4\MinaiAI"

REM 启动前端
timeout /t 10 /nobreak >nul
start /min "前端服务" cmd /c "cd /d "%ROOT%\2\assessweb" && npm run dev"

REM 打开浏览器
timeout /t 8 /nobreak >nul
start http://localhost:5173/login

exit
