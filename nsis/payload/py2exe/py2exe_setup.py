from distutils.core import setup
import py2exe
setup(console=['audiofarm_resolver.py',
               'echonest-resolver.py',
               'mp3tunes-resolver.py',
               'napster_resolver.py'])
