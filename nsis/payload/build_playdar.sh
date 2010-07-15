#!/bin/sh
#
# This is used with Cygwin bash.
#
# Playdar binaries are not currently copied with this script.
# Optional modules and py2exe are provided by this script.

BUILD_DIR=`pwd`

PLAYDAR_REL_PATH=../../../Playdar
PLAYDAR_HOME=`cd $PLAYDAR_REL_PATH; pwd`

# Refresh playdar modules in payload folder.
cd $PLAYDAR_HOME
rm -rf playdar_modules/aolmusic
rm -rf playdar_modules/jamendo
rm -rf playdar_modules/magnatune
rm -rf playdar_modules/player
cp -R contrib/aolmusic playdar_modules
cp -R contrib/jamendo playdar_modules
cp -R contrib/magnatune_module playdar_modules/magnatune
cp -R contrib/player playdar_modules
make clean
make
rm -rf ../../payload/playdar_modules
mkdir -p ../../payload/playdar_modules/aolmusic/ebin
mkdir -p ../../payload/playdar_modules/jamendo/ebin
mkdir -p ../../payload/playdar_modules/magnatune/ebin
mkdir -p ../../payload/playdar_modules/player/ebin
cp -R playdar_modules/aolmusic/ebin ../../payload/playdar_modules/aolmusic
cp -R playdar_modules/jamendo/ebin ../../payload/playdar_modules/jamendo
cp -R playdar_modules/magnatune/ebin ../../payload/playdar_modules/magnatune
cp -R playdar_modules/audioscrobbler/ebin ../../payload/playdar_modules/audioscrobbler
cp -R playdar_modules/player/ebin ../../payload/playdar_modules/player
rm -rf playdar_modules/aolmusic
rm -rf playdar_modules/jamendo
rm -rf playdar_modules/magnatune
rm -rf playdar_modules/player

# Remove the old py2exe compilation artifacts.
#rm -rf contrib/amiestreet/build contrib/amiestreet/dist 
#rm -rf contrib/audiofarm/build contrib/audiofarm/dist
#rm -rf contrib/echonest/build contrib/echonest/dist
#rm -rf contrib/mp3tunes/build contrib/mp3tunes/dist
#rm -rf contrib/napster/build contrib/napster/dist
#find contrib/amiestreet -name *.pyc -exec rm -rf {} \;
#find contrib/audiofarm -name *.pyc -exec rm -rf {} \;
#find contrib/echonest -name *.pyc -exec rm -rf {} \;
#find contrib/mp3tunes -name *.pyc -exec rm -rf {} \;
#find contrib/napster -name *.pyc -exec rm -rf {} \;

# Refresh playdar contrib scripts in payload folder.
rm -r ../../payload/playdar_python_resolvers/*.pyc
#cp contrib/amiestreet/amiestreet-resolver.py ../../payload/playdar_python_resolvers
cp contrib/audiofarm/audiofarm_resolver.py ../../payload/playdar_python_resolvers
cp contrib/echonest/echonest-resolver.py ../../payload/playdar_python_resolvers
cp -R contrib/echonest/pyechonest ../../payload/playdar_python_resolvers
cp contrib/mp3tunes/mp3tunes-resolver.py ../../payload/playdar_python_resolvers
cp contrib/napster/napster_resolver.py ../../payload/playdar_python_resolvers
cp contrib/napster/napster.py ../../payload/playdar_python_resolvers
cp -R contrib/resolver_libs/simplejson ../../payload/playdar_python_resolvers
cp contrib/resolver_libs/playdar_resolver.py ../../payload/playdar_python_resolvers

# Re-compile python contrib scripts with py2exe.
cd ../../payload/playdar_python_resolvers
/cygdrive/c/Python26/python.exe py2exe_setup.py py2exe

cd ../../Packages
