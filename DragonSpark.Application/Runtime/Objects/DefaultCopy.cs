using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Application.Runtime.Objects;

public sealed class DefaultCopy<T> : Copy<T> where T : notnull
{
    public static DefaultCopy<T> Default { get; } = new();

    DefaultCopy() : base(DefaultSerializer<T>.Default) {}
}

public sealed class Copied<T> : Result<T?>, IMutable<T?> where T : notnull
{
    readonly IMutable<T?>   _previous;
    readonly IAlteration<T> _copy;

    public Copied() : this(new Variable<T>(), DefaultCopy<T>.Default) {}

    public Copied(IMutable<T?> previous, IAlteration<T> copy) : base(previous)
    {
        _previous = previous;
        _copy     = copy;
    }

    public void Execute(T? parameter)
    {
        var instance = parameter is not null ? _copy.Get(parameter) : default;
        _previous.Execute(instance);
    }
}