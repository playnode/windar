namespace Windar.Common
{
    public class Credentials
    {
        string _username;
        string _password;

        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public Credentials(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
