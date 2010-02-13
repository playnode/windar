Windar
======
Windar is an GPL-licensed package for Playdar installation on Windows.
For more information see: http://www.windar.org/

0.1.3 Release Notes
-------------------
A new user data folder is created in each release to avoid complications.
The previous data can be recovered from the path "%AppData%\Windar2".
An automated upgrade feature will be developed later. In the meantime
most users should find it acceptable to re-index their library and start
with a fresh copy of the default configuration files.

Playdar
=======
Playdar is a music content resolver, written in Erlang, MIT licensed.
It's modular and can be extended using resolver scripts in any language.
It has an HTTP API to answer the question "can i play this song?".
If yes, you get a localhost URL that will play the song, from whichever
source playdar resolved it to (plugins do the resolving).

For more information see: http://www.playdar.org/

This product includes software developed by the OpenSSL Project for use in
the OpenSSL Toolkit (http://www.openssl.org/)