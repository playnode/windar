#!/bin/bash -e
set -x
ORIG="/cygdrive/c/Users/Steve/Desktop/Playdar/erlang/erl5.7.3-bin"
DEST="/cygdrive/c/Users/Steve/Desktop/Playdar/erlang/erl5.7.3-min"

cd "$ORIG"
rm -rf "$DEST"
mkdir -p "$DEST"

# The basic bits
cp -r bin "$DEST/"
cp -r releases "$DEST/"
cp -r erts-* "$DEST/"
cp vcredist* "$DEST/"

# The libs:
mkdir -p "$DEST/lib"
cd lib

libs="inets* kernel* crypto* sasl* stdlib* syntax_tools* compiler*"
for libdir in $libs
do
	mkdir -p "$DEST/lib/$libdir"
	if [ -d "$libdir/ebin" ]
	then
		cp -r "$libdir/ebin" "$DEST/lib/$libdir/"
	fi
	if [ -d "$libdir/include" ]
	then
		cp -r "$libdir/include" "$DEST/lib/$libdir/"
	fi
	if [ -d "$libdir/priv" ]
	then
		cp -r "$libdir/priv" "$DEST/lib/$libdir/"
	fi
done

echo "Finished."