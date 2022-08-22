using System.Net;
using System.Text;
using System.Web;

namespace WannaEat.Shared;

public class UriQueryBuilder
{
    private readonly List<KeyValuePair<string, string>> _query;

    public IReadOnlyList<KeyValuePair<string, string>> Queries
    {
        get => _query;
        init => _query = value.ToList();
    }

    public UriQueryBuilder(IEnumerable<KeyValuePair<string, string>> query)
    {
        _query = query.ToList();
    }

    public UriQueryBuilder() : this(Enumerable.Empty<KeyValuePair<string, string>>())
    { }

    public string Query => Build();

    public UriQueryBuilder Append(IEnumerable<KeyValuePair<string, string>> queries)
    {
        foreach (var query in queries)
        {
            Append(query);
        }

        return this;
    }
    
    public UriQueryBuilder Append(KeyValuePair<string, string> query)
    {
        var (name, value) = query;
        return Append(name, value);
    }

    private UriQueryBuilder Append(string name, string value)
    {
        _query.Add(new KeyValuePair<string, string>(name, value));
        return this;
    }

    private string Build()
    {
        if (_query.Count == 0)
            return string.Empty;
        
        var builder = new StringBuilder();
        _query.ForEach(AddQuery);
        
        // At least 1 element must exist
        builder.Remove(0, 1);
        return builder.ToString();
        
        string Encode(string str) => HttpUtility.UrlEncode(str);
        void AddQuery(KeyValuePair<string, string> query)
        {
            var (name, value) = query;
            builder.Append('&')
                   .Append(Encode(name))
                   .Append('=')
                   .Append(Encode(value));
        }
    }
}