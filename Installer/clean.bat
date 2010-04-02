@ECHO OFF
ECHO Cleaning Common
RMDIR /S /Q "..\Common\bin"
RMDIR /S /Q "..\Common\obj"
ECHO Cleaning ErlangTerms
RMDIR /S /Q "..\Libraries\ErlangTerms\Parser\bin"
RMDIR /S /Q "..\Libraries\ErlangTerms\Parser\obj"
ECHO Cleaning Installer\Utils\erlini
RMDIR /S /Q "Utils\erlini\bin"
RMDIR /S /Q "Utils\erlini\obj"
ECHO Cleaning PlaydarDaemon
RMDIR /S /Q "..\PlaydarDaemon\bin"
RMDIR /S /Q "..\PlaydarDaemon\obj"
ECHO Cleaning PluginAPI
RMDIR /S /Q "..\PluginAPI\bin"
RMDIR /S /Q "..\PluginAPI\obj"
ECHO Cleaning Plugins\Electronome
RMDIR /S /Q "..\Plugins\Electronome\bin"
RMDIR /S /Q "..\Plugins\Electronome\obj"
ECHO Cleaning Plugins\MP3tunes
RMDIR /S /Q "..\Plugins\MP3tunes\bin"
RMDIR /S /Q "..\Plugins\MP3tunes\obj"
ECHO Cleaning Plugins\Napster
RMDIR /S /Q "..\Plugins\Napster\bin"
RMDIR /S /Q "..\Plugins\Napster\obj"
ECHO Cleaning Plugins\Player
RMDIR /S /Q "..\Plugins\Player\bin"
RMDIR /S /Q "..\Plugins\Player\obj"
ECHO Cleaning Plugins\Scrobbler
RMDIR /S /Q "..\Plugins\Scrobbler\bin"
RMDIR /S /Q "..\Plugins\Scrobbler\obj"
ECHO Cleaning TrayApp
RMDIR /S /Q "..\TrayApp\bin"
RMDIR /S /Q "..\TrayApp\obj"
ECHO Cleaning TrayAppTest
RMDIR /S /Q "..\TrayAppTest\bin"
RMDIR /S /Q "..\TrayAppTest\obj"
ECHO Cleaning Temp
RMDIR /S /Q "Temp"
REM ECHO Cleaning Utils\erlini
REM RMDIR /S /Q "Utils\erlini\bin"
REM RMDIR /S /Q "Utils\erlini\obj"
ECHO Done.
