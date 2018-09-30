using System;

namespace Windar.Common
{
    public class WindarException : Exception
    {
        public WindarException(string str) : base(str) { }
    }
}
