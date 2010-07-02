@ECHO OFF
ECHO Cleaning Common
RMDIR /S /Q "..\src\Common\bin"
RMDIR /S /Q "..\src\Common\obj"

ECHO Cleaning ErlangTerms
RMDIR /S /Q "..\src\ErlangTerms\bin"
RMDIR /S /Q "..\src\ErlangTerms\obj"

ECHO Cleaning PlaydarDaemon
RMDIR /S /Q "..\src\PlaydarDaemon\bin"
RMDIR /S /Q "..\src\PlaydarDaemon\obj"

ECHO Cleaning PluginAPI
RMDIR /S /Q "..\src\PluginAPI\bin"
RMDIR /S /Q "..\src\PluginAPI\obj"

ECHO Cleaning Plugins\MP3tunes
RMDIR /S /Q "..\src\Plugins\MP3tunes\bin"
RMDIR /S /Q "..\src\Plugins\MP3tunes\obj"

ECHO Cleaning Plugins\Napster
RMDIR /S /Q "..\src\Plugins\Napster\bin"
RMDIR /S /Q "..\src\Plugins\Napster\obj"

ECHO Cleaning Plugins\Player
RMDIR /S /Q "..\src\Plugins\Player\bin"
RMDIR /S /Q "..\src\Plugins\Player\obj"

ECHO Cleaning Plugins\Scrobbler
RMDIR /S /Q "..\src\Plugins\Scrobbler\bin"
RMDIR /S /Q "..\src\Plugins\Scrobbler\obj"

ECHO Cleaning TrayApp
RMDIR /S /Q "..\src\TrayApp\bin"
RMDIR /S /Q "..\src\TrayApp\obj"

ECHO Cleaning TrayAppTest
RMDIR /S /Q "..\src\TrayAppTest\bin"
RMDIR /S /Q "..\src\TrayAppTest\obj"

ECHO Cleaning Temp
RMDIR /S /Q "temp"

REM ECHO Cleaning erlini
REM RMDIR /S /Q "erlini\bin"
REM RMDIR /S /Q "erlini\obj"

ECHO Done.