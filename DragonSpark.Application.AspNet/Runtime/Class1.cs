using System.Text;
using DragonSpark.Application.Runtime.Objects;
using DragonSpark.Model.Results;
using DragonSpark.Text;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Runtime;

internal class Class1 {}

public class SessionVariable : SessionVariable<string>
{
    protected SessionVariable(HttpContext context, string key) : base(context.Session, key, Serializer.Default) {}

    sealed class Serializer : Serializer<string>
    {
        public static Serializer Default { get; } = new();

        Serializer() : base(Formatter.Instance, SelfParser.Instance, SelfTarget.Instance) {}

        sealed class Formatter : Formatter<string>
        {
            public static Formatter Instance { get; } = new();

            Formatter() : base(x => x) {}
        }

        sealed class SelfParser : Parser<string>
        {
            public static SelfParser Instance { get; } = new();

            SelfParser() : base(x => x) {}
        }

        sealed class SelfTarget : ITarget<string>
        {
            public static SelfTarget Instance { get; } = new();

            SelfTarget() {}

            public void Execute(TargetInput<string> parameter) {}
        }
    }
}

public class SessionVariable<T> : IMutable<T?> where T : notnull
{
    readonly ISession       _session;
    readonly string         _key;
    readonly ISerializer<T> _serializer;

    protected SessionVariable(HttpContext context, string key)
        : this(context.Session, key, DefaultSerializer<T>.Default) {}

    protected SessionVariable(ISession session, string key, ISerializer<T> serializer)
    {
        _session    = session;
        _key        = key;
        _serializer = serializer;
    }

    public T? Get() => _session.TryGetValue(_key, out var value) ? _serializer.Get(value) : default;

    public void Execute(T? parameter)
    {
        if (parameter is not null)
        {
            _session.Set(_key, Encoding.UTF8.GetBytes(_serializer.Get(parameter)));
        }
        else
        {
            _session.Remove(_key);
        }
    }
}