@echo off
cls
.paket\paket.exe restore
if errorlevel 1 (
    exit /b %errorlevel%
)

"packages\tools\FAKE\tools\Fake" build.fsx %*
pause