namespace Windar.ErlangTermsParser
{
    public class FloatToken : NumericExpression
    {
        public FloatToken()
        {
            
        }

        public FloatToken(string text)
        {
            Text = text;
        }

        public FloatToken(float num)
        {
            Text = num.ToString();
        }
    }
}
