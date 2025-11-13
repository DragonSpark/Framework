using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using DragonSpark.Compose;
using DragonSpark.Text;

namespace DragonSpark.Application.Communication.Http.Messaging;

public sealed class ParseForm : IParser<FormUrlEncodedContent>
{
    readonly IAmbientProperties                        _add;
    readonly IParser<Dictionary<string, List<string>>> _parser;

    public ParseForm(IAmbientProperties add) : this(add, ParseEntries.Default) {}

    public ParseForm(IAmbientProperties add, IParser<Dictionary<string, List<string>>> parser)
    {
        _add    = add;
        _parser = parser;
    }

    public FormUrlEncodedContent Get(string parameter)
    {
        var form = _parser.Get(parameter);
        foreach (var (key, value) in _add.Get())
        {
            if (form.TryGetValue(key, out var list))
            {
                list.Add(value);
            }
            else
            {
                form[key] = [value];
            }
        }

        return new(form.SelectMany(x => x.Value.Select(v => x.Key.Map(v))));
    }
}