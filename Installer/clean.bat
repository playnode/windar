@ECHO OFF
ECHO Cleaning Common
RMDIR /S /Q "..\Common\bin"
RMDIR /S /Q "..\Common\obj"
ECHO Cleaning PlaydarDaemon
RMDIR /S /Q "..\PlaydarDaemon\bin"
RMDIR /S /Q "..\PlaydarDaemon\obj"
ECHO Cleaning PluginAPI
RMDIR /S /Q "..\PluginAPI\bin"
RMDIR /S /Q "..\PluginAPI\obj"
ECHO Cleaning Plugins\Electronome
RMDIR /S /Q "..\Plugins\Electronome\bin"
RMDIR /S /Q "..\Plugins\Electronome\obj"
ECHO Cleaning Plugins\Scrobbler
RMDIR /S /Q "..\Plugins\Scrobbler\bin"
RMDIR /S /Q "..\Plugins\Scrobbler\obj"
ECHO Cleaning TrayApp
RMDIR /S /Q "..\TrayApp\bin"
RMDIR /S /Q "..\TrayApp\obj"
ECHO Cleaning Temp
RMDIR /S /Q "Temp"
REM ECHO Cleaning Utils\erlini
REM RMDIR /S /Q "Utils\erlini\bin"
REM RMDIR /S /Q "Utils\erlini\obj"
ECHO Done.
