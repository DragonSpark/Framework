using System.Linq;

namespace DragonSpark.Application.AspNet.Security.Tokens;

sealed class TypeAwareComposeQuery : IComposeQuery
{
    readonly ComposeQuery _previous;

    public TypeAwareComposeQuery(ComposeQuery previous) => _previous = previous;

    public ComposeQueryResult Get(ComposeQueryInput parameter)
    {
        var (_, _, type) = parameter;
        var previous = _previous.Get(parameter);
        return type is not null ? previous with { Query = previous.Query.Where(n => n.Purpose == type.Value) } : previous;
    }
}