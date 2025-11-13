using System.Collections.Generic;
using System.Net.Http;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Communication.Http;

sealed class GetHttpContent<T> : ISelect<T, FormUrlEncodedContent>
{
    public static GetHttpContent<T> Default { get; } = new();

    GetHttpContent() : this(Deconstruct.Default, Deconstruct<T>.Default) {}

    readonly IDeconstruct    _map;
    readonly IDeconstruct<T> _instance;

    public GetHttpContent(IDeconstruct map, IDeconstruct<T> instance)
    {
        _map      = map;
        _instance = instance;
    }

    public FormUrlEncodedContent Get(T parameter)
    {
        var form = parameter as IEnumerable<KeyValuePair<string, string>> ??
                   (parameter is IDictionary<string, object?> map ? _map.Get(map) : _instance.Get(parameter));
        return new(form);
    }
}