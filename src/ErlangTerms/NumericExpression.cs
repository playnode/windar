namespace Playnode.ErlangTerms.Parser
{
    public class NumericExpression : ParserToken, IValueToken
    {
        string _text;

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
