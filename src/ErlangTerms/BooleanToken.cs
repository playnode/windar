using System;

namespace Windar.ErlangTermsParser
{
    public class BooleanToken : AtomToken
    {
        public bool Value
        {
            get { return Convert.ToBoolean(Text); }
            set { Text = value.ToString().ToLower(); }
        }

        public BooleanToken(bool value)
        {
            Value = value;
        }

        public BooleanToken(string str)
        {
            switch (str.ToLower())
            {
                case "true":
                    Value = true;
                    break;
                case "false":
                    Value = false;
                    break;
                default:
                    throw new ArgumentException("String is not \"true\" or \"false\".");
            }
        }

        public BooleanToken(AtomToken token)
        {
            Value = Convert.ToBoolean(token.Text);
        }

        public override string ToString()
        {
            return Value ? "true" : "false";
        }
    }
}
