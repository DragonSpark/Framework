using DragonSpark.Model;

namespace DragonSpark.Composition.Compose;

public abstract class HostAccessor<T> : IHostAccessor<T> where T : class
{
    readonly object _key;

    protected HostAccessor(object key) => _key = key;

    public T? Get(IDictionary<object, object> parameter)
        => parameter.TryGetValue(_key, out var value) && value is T result ? result : null;

    public void Execute(Pair<IDictionary<object, object>, T> parameter)
    {
        var (key, value) = parameter;
        key[_key]        = value;
    }
}