#!/bin/sh
#
# This is used with Cygwin bash.
#
# Playdar binaries are not currently copied with this script.
# Optional modules and py2exe are provided by this script.

BUILD_DIR=`pwd`

PLAYDAR_REL_PATH=../../../playdar-core
PLAYDAR_HOME=`cd $PLAYDAR_REL_PATH; pwd`
cd $PLAYDAR_HOME

# Refresh playdar modules in payload folder.
rm -rf playdar_modules/aolmusic
rm -rf playdar_modules/jamendo
rm -rf playdar_modules/magnatune
cp -R contrib/aolmusic playdar_modules
cp -R contrib/jamendo playdar_modules
cp -R contrib/magnatune_module playdar_modules/magnatune
make clean
make
rm -rf ../windar/nsis/payload/playdar_modules
mkdir -p ../windar/nsis/payload/playdar_modules/aolmusic/ebin
mkdir -p ../windar/nsis/payload/playdar_modules/jamendo/ebin
mkdir -p ../windar/nsis/payload/playdar_modules/magnatune/ebin
cp -R playdar_modules/aolmusic/ebin ../windar/nsis/payload/playdar_modules/aolmusic
cp -R playdar_modules/jamendo/ebin ../windar/nsis/payload/playdar_modules/jamendo
cp -R playdar_modules/magnatune/ebin ../windar/nsis/payload/playdar_modules/magnatune
cp -R playdar_modules/audioscrobbler/ebin ../windar/nsis/payload/playdar_modules/audioscrobbler
rm -rf playdar_modules/aolmusic
rm -rf playdar_modules/jamendo
rm -rf playdar_modules/magnatune

# Refresh playdar contrib scripts in payload folder.
rm -rf ../windar/nsis/payload/playdar_python_resolvers/build
rm -rf ../windar/nsis/payload/playdar_python_resolvers/dist
find ../windar/nsis/payload/playdar_python_resolvers -name *.pyc -exec rm {} \;
cp contrib/audiofarm/audiofarm_resolver.py ../windar/nsis/payload/playdar_python_resolvers/
cp contrib/echonest/echonest-resolver.py ../windar/nsis/payload/playdar_python_resolvers/
cp -R contrib/echonest/pyechonest ../windar/nsis/payload/playdar_python_resolvers/
cp contrib/mp3tunes/mp3tunes-resolver.py ../windar/nsis/payload/playdar_python_resolvers/
cp contrib/napster/napster_resolver.py ../windar/nsis/payload/playdar_python_resolvers/
cp contrib/napster/napster.py ../windar/nsis/payload/playdar_python_resolvers/
cp -R contrib/resolver_libs/simplejson ../windar/nsis/payload/playdar_python_resolvers/
cp contrib/resolver_libs/playdar_resolver.py ../windar/nsis/payload/playdar_python_resolvers/

# Re-compile python contrib scripts with py2exe.
cd ../windar/nsis/payload/playdar_python_resolvers
/cygdrive/c/Python26/python.exe py2exe_setup.py py2exe

cd $BUILD_DIR