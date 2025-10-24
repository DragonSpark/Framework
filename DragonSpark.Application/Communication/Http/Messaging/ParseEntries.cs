using System;
using System.Collections.Generic;
using System.Web;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Text;

namespace DragonSpark.Application.Communication.Http.Messaging;

sealed class ParseEntries : IParser<Dictionary<string, List<string>>>
{
    public static ParseEntries Default { get; } = new();

    ParseEntries() : this(StringSplitOptions.RemoveEmptyEntries) {}

    readonly StringSplitOptions _options;

    public ParseEntries(StringSplitOptions options) => _options = options;

    public Dictionary<string, List<string>> Get(string parameter)
    {
        var result = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
        foreach (var pair in !parameter.IsNullOrEmpty() ? parameter.Split('&', _options) : Empty.Array<string>())
        {
            var parts = pair.Split('=', 2, _options);
            switch (parts.Length)
            {
                case 2:
                {
                    var key   = HttpUtility.UrlDecode(parts[0]);
                    var value = HttpUtility.UrlDecode(parts[1]);
                    if (!result.TryGetValue(key, out var values))
                    {
                        values      = [];
                        result[key] = values;
                    }

                    values.Add(value);
                    break;
                }
            }
        }

        return result;
    }
}