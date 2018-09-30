using System;
using System.Text;

namespace Windar.ErlangTermsParser
{
    public class AddedComment : CommentToken
    {
        internal static string Timestamp
        {
            get
            {
                StringBuilder result = new StringBuilder();
                result.Append(DateTime.Now.ToShortTimeString()).Append(", ");
                result.Append(DateTime.Now.ToLongDateString()).Append('.');
                return result.ToString();
            }
        }

        public const string AddedCommentBegin = "% Added at ";

        public AddedComment() : base(AddedCommentBegin + Timestamp) { }
    }
}
