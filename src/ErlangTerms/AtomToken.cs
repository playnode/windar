namespace Windar.ErlangTermsParser
{
    public class AtomToken : ParserToken, IValueToken
    {
        string _text;

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public AtomToken()
        {
            
        }

        public AtomToken(string text)
        {
            Text = text;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
