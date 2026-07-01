@echo off
chcp 65001 >nul
title 🔐 管理端启动中...

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

REM 启动后台管理
timeout /t 10 /nobreak >nul
start /min "后台管理" cmd /c "cd /d "%ROOT%\1" && npm run dev"

REM 打开登录页
timeout /t 15 /nobreak >nul
start http://localhost:9527/turnLogin

exit
