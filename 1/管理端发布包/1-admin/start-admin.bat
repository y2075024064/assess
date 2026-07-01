@echo off
chcp 65001 >nul
title 后台管理启动中...

set ROOT=%~dp0.

REM 确保 MySQL 已启动
echo 正在检查 MySQL...
sc query MySQL80 | find "RUNNING" >nul
if errorlevel 1 (
    echo MySQL 未运行，正在启动...
    net start MySQL80 >nul 2>&1
)
:wait_mysql
sc query MySQL80 | find "RUNNING" >nul
if errorlevel 1 (
    timeout /t 2 /nobreak >nul
    goto wait_mysql
)
echo MySQL 已就绪，正在启动后台管理...

start /min "后台管理" cmd /c "cd /d "%ROOT%" && npm run dev"

timeout /t 15 /nobreak >nul
start http://localhost:9527/turnLogin

exit
