Playdar
=======
Playdar is a music content resolver, written in Erlang, MIT licensed.
It's modular and can be extended using resolver scripts in any language.
It has an HTTP API to answer the question "can i play this song?".
If yes, you get a localhost URL that will play the song, from whichever
source playdar resolved it to (plugins do the resolving).

This product includes software developed by the OpenSSL Project for use in
the OpenSSL Toolkit ([www.openssl.org](http://www.openssl.org/)).

For more information see: [www.playdar.org](http://www.playdar.org/)

Windar
======
[Windar](http://windar.org/) is an GPL-licensed package for [Playdar](http://www.playdar.org/) installation on Windows.

0.1.3 Release Notes
-------------------
A new user data folder is created in each release to avoid complications.
The previous data can be recovered from the path "%AppData%\Windar2".
An automated upgrade feature will be developed later. In the meantime
most users should find it acceptable to re-index their library and start
with a fresh copy of the default configuration files.

Developing
----------
Steps to rebuild:

-   Use [7-zip](http://www.7-zip.org/) to unpack the following to same directory.

    -   Installer\Payload\libeay32.7z

    -   Installer\Payload\minimerl.7z

    -   Installer\Payload\mplayer.7z

    -   Installer\Payload\playdar.7z

    -   Installer\Payload\playdar_python_resolvers\dist.7z

-   Open the Windar solution file in Visual Studio 2008 Express (free download).
    Allow modified project file to load normally. Versioning requires build task which results
    in a warning when you open each versioned project.