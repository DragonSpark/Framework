using DragonSpark.Runtime;

namespace DragonSpark.Application.AspNet.Security.Tokens;

sealed class ComposeQuery : IComposeQuery
{
    public static ComposeQuery Default { get; } = new();

    readonly ITime _time;

    public ComposeQuery() : this(Time.Default) {}

    public ComposeQuery(ITime time) => _time = time;

    public ComposeQueryResult Get(ComposeQueryInput parameter)
    {
        var (source, identity) = parameter;
        var now = _time.Get().UtcDateTime;
        return new(source.Where(n => n.Key == identity && n.UsedAtUtc == null && n.ExpiresAtUtc >= now), now);
    }
}