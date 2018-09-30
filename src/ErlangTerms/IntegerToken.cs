using System;

namespace Playnode.ErlangTerms.Parser
{
    public class IntegerToken : NumericExpression
    {
        public int Value
        {
            get { return Int32.Parse(Text); }
            set { Text = value.ToString(); }
        }

        public IntegerToken()
        {
            
        }

        public IntegerToken(string text)
        {
            Text = text;
        }

        public IntegerToken(int num)
        {
            Text = num.ToString();
        }
    }
}
