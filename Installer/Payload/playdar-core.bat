@ECHO OFF

SET PLAYDAR_HOME=%~dp0
SET PLAYDAR_ETC=%AppData%\Windar\etc

IF NOT EXIST "%AppData%\Windar" MKDIR "%AppData%\Windar"
IF NOT EXIST "%AppData%\Windar\etc" MKDIR "%AppData%\Windar\etc"
IF NOT EXIST "%AppData%\Windar\etc\playdar.conf" COPY "%PLAYDAR_HOME%etc\playdar.conf" %PLAYDAR_ETC%
IF NOT EXIST "%AppData%\Windar\etc\playdartcp.conf" COPY "%PLAYDAR_HOME%etc\playdartcp.conf" %PLAYDAR_ETC%

CD %AppData%\Windar

SET PLAYDAR_CMD="%PLAYDAR_HOME%..\minimerl\bin\erl.exe"
SET PLAYDAR_CMD=%PLAYDAR_CMD% -sname playdar@localhost
REM SET PLAYDAR_CMD=%PLAYDAR_CMD% -noinput
SET PLAYDAR_CMD=%PLAYDAR_CMD% -pa "%PLAYDAR_HOME%ebin"
SET PLAYDAR_CMD=%PLAYDAR_CMD% -boot start_sasl
SET PLAYDAR_CMD=%PLAYDAR_CMD% -s reloader
SET PLAYDAR_CMD=%PLAYDAR_CMD% -s playdar

%PLAYDAR_CMD%

ECHO.
ECHO.
ECHO Erlang session ended.
PAUSE