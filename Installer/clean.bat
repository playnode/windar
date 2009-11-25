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
ECHO Cleaning Plugins\PlayerPlugin
RMDIR /S /Q "..\Plugins\PlayerPlugin\bin"
RMDIR /S /Q "..\Plugins\PlayerPlugin\obj"
ECHO Cleaning TrayApp
RMDIR /S /Q "..\TrayApp\bin"
RMDIR /S /Q "..\TrayApp\obj"
ECHO Cleaning Temp
RMDIR /S /Q "Temp"
ECHO Cleaning Utils\erlini
RMDIR /S /Q "Utils\erlini\bin"
RMDIR /S /Q "Utils\erlini\obj"
ECHO Done.
