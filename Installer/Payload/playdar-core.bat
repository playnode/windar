@ECHO OFF

SET WINDAR_DIR=%~dp0

IF NOT EXIST "%AppData%\Windar" MKDIR "%AppData%\Windar"
IF NOT EXIST "%AppData%\Windar\etc" XCOPY /E "%WINDAR_DIR%playdar\etc" "%AppData%\Windar\etc\"
IF NOT EXIST "%AppData%\Windar\priv" XCOPY /E "%WINDAR_DIR%playdar\priv" "%AppData%\Windar\priv\"
IF NOT EXIST "%AppData%\Windar\playdar_modules" XCOPY /E "%WINDAR_DIR%playdar\playdar_modules" "%AppData%\Windar\playdar_modules\"

CD "%AppData%\Windar"
SET WINDAR_CMD="%WINDAR_DIR%minimerl\bin\erl.exe"
SET WINDAR_CMD=%WINDAR_CMD% -sname playdar@localhost
REM SET WINDAR_CMD=%WINDAR_CMD% -noinput
SET WINDAR_CMD=%WINDAR_CMD% -pa "%WINDAR_DIR%playdar\ebin"
SET WINDAR_CMD=%WINDAR_CMD% -boot start_sasl
SET WINDAR_CMD=%WINDAR_CMD% -s reloader
SET WINDAR_CMD=%WINDAR_CMD% -s playdar

%WINDAR_CMD%

ECHO.
ECHO.
ECHO Erlang session ended.
PAUSE