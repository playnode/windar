#!/bin/sh

# Refresh playdar modules in payload folder.
cd Playdar
rm -rf playdar_modules/aolmusic
rm -rf playdar_modules/jamendo
rm -rf playdar_modules/magnatune
cp -R contrib/aolmusic playdar_modules
cp -R contrib/jamendo playdar_modules
cp -R contrib/magnatune_module playdar_modules/magnatune
make clean
make
rm -rf ../../Payload/playdar_modules
mkdir -p ../../Payload/playdar_modules/aolmusic/ebin
mkdir -p ../../Payload/playdar_modules/jamendo/ebin
mkdir -p ../../Payload/playdar_modules/magnatune/ebin
cp -R playdar_modules/aolmusic/ebin ../../Payload/playdar_modules/aolmusic
cp -R playdar_modules/jamendo/ebin ../../Payload/playdar_modules/jamendo
cp -R playdar_modules/magnatune/ebin ../../Payload/playdar_modules/magnatune
rm -rf playdar_modules/aolmusic
rm -rf playdar_modules/jamendo
rm -rf playdar_modules/magnatune

# Re-compile python contrib scripts with py2exe.
rm -rf contrib/amiestreet/build contrib/amiestreet/dist
rm -rf contrib/audiofarm/build contrib/audiofarm/dist
rm -rf contrib/echonest/build contrib/echonest/dist
rm -rf contrib/mp3tunes/build contrib/mp3tunes/dist
rm -rf contrib/napster/build contrib/napster/dist
cd contrib/amiestreet
/cygdrive/c/Python26/python.exe setup.py py2exe
cd ../../contrib/audiofarm
/cygdrive/c/Python26/python.exe setup.py py2exe
cd ../../contrib/echonest
/cygdrive/c/Python26/python.exe setup.py py2exe
cd ../../contrib/mp3tunes
/cygdrive/c/Python26/python.exe setup.py py2exe
cd ../../contrib/napster
/cygdrive/c/Python26/python.exe setup.py py2exe
cd ../../

# Refresh playdar contrib scripts in payload folder.
rm -rf ../../Payload/playdar_scripts
mkdir ../../Payload/playdar_scripts
cp -R contrib/amiestreet/dist ../../Payload/playdar_scripts/amiestreet
cp -R contrib/audiofarm/dist ../../Payload/playdar_scripts/audiofarm
cp -R contrib/echonest/dist ../../Payload/playdar_scripts/echonest
cp -R contrib/mp3tunes/dist ../../Payload/playdar_scripts/mp3tunes
cp -R contrib/napster/dist ../../Payload/playdar_scripts/napster
