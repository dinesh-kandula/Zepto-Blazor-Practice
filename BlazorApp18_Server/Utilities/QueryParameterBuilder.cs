namespace BlazorApp18_Server.Utilities
{
    public class QueryParameterBuilder
    {
        private readonly List<string> _parameters = [];

        public QueryParameterBuilder Add(string key, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                _parameters.Add($"{Uri.EscapeDataString(key)}={Uri.EscapeDataString(value)}");
            }
            return this;
        }

        public string Build()
        {
            return string.Join("&", _parameters);
        }
    }
}
