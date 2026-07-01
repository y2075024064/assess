# 后台管理启动脚本
$Root = (Get-Item $PSScriptRoot).Directory.FullName

Start-Process powershell -ArgumentList "-WindowStyle Hidden", "-Command", "cd '$Root'; npm run dev" -WindowStyle Hidden

Start-Sleep -Seconds 15
Start-Process "http://localhost:9527/turnLogin"
