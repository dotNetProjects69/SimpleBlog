namespace SimpleBlog.Models
{
    public class Search
    {
        private string _nickname = string.Empty;
        private List<string> _result = new();

        public string Nickname { get => _nickname; set => _nickname = value; }
        public List<string> Result { get => _result; set => _result = value; }
    }
}
