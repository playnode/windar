Playdar
=======
Playdar is a music content resolver, written in Erlang, MIT licensed.
It's modular and can be extended using resolver scripts in any language.
It has an HTTP API to answer the question "can i play this song?".
If yes, you get a localhost URL that will play the song, from whichever
source playdar resolved it to (plugins do the resolving).

For more information see: [www.playdar.org](http://www.playdar.org/)

This product includes software developed by the OpenSSL Project for use in
the OpenSSL Toolkit ([www.openssl.org](http://www.openssl.org/)).

Windar
======
[Windar](http://windar.org/) is an GPL-licensed package for 
[Playdar](http://www.playdar.org/) installation on Windows.

Developing
----------
How to get started when developing Windar:

-   Use [7-zip](http://www.7-zip.org/) to unpack the contents of the following
    archive files to same directory. 

    -   Installer\Payload\libeay32.7z
        (pre-compiled [OpenSSL for Windows](http://gnuwin32.sourceforge.net/packages/openssl.htm))
    -   Installer\Payload\minimerl.7z
        (pre-compiled, cut-down version of [Erlang](http://www.erlang.org/))
    -   Installer\Payload\mplayer.7z
        (pre-compiled, cut-down version of [MPlayer](http://www.mplayerhq.hu/))
    -   Installer\Payload\playdar.7z
        (pre-compiled [Playdar](http://www.playdar.org/) core distribution files)
    -   Installer\Payload\playdar_python_resolvers\dist.7z
        (set of Python resolvers for [Playdar](http://www.playdar.org/), pre-compiled with py2exe)

-   Open the Windar solution file in Visual C# 2008 Express Edition. You should
    find this as a free download [here](http://www.microsoft.com/express/downloads/).
    You can allow the modified TrayApp project file to load normally and ignore
    warning due to the included build task, used to auto-increment the DLL version.

-   Click on the "Set as StartUp Project" option in the context menu for the
    TrayApp project in the Solution Explorer panel.

-   Start Debugging (F5)

-   Click on the Shutdown option in the Windar tray icon context menu to end.

If there are problems preventing a gracefull shutdown, or other software crash,
you'll likely find that you need to kill the erl.exe and epmd.exe processes
which are related to Erlang and playdar-core.

It is normal for the epmd.exe process to remain running, however it will lock
files in the Windar solution folder, so you may need to kill that process to
have a clean start or when deleting or moving the folder.

Installer build
---------------
It is possible to compile Windar and build the installer without Visual Studio
using the supplied build.bat file in the Installer folder. However, you'll
need the following:

-   .NET Framework 2.0
    [x86](http://www.microsoft.com/downloads/details.aspx?FamilyID=0856eacb-4362-4b0d-8edd-aab15c5e04f5&displaylang=en)
    [x64](http://www.microsoft.com/downloads/details.aspx?familyid=B44A0000-ACF8-4FA1-AFFB-40E78D788B00&displaylang=en)
    or [.NET Framework 3.5](http://www.microsoft.com/downloads/details.aspx?familyid=333325FD-AE52-4E35-B531-508D977D32A6&displaylang=en)
    One or both will typically be available on XP, Vista, and Windows 7.
-   [NSIS](http://nsis.sourceforge.net/Download) is a 
    scriptable win32 installer/uninstaller system.

[7-zip](http://www.7-zip.org/) must still be used to unpack the contents some
pre-compiled software as described in the section "Developing" above.