using DragonSpark.Compose;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Additional;

public class SelectedProperties<T> : IProperties
{
    readonly Func<T, object> _select;
    readonly IProperties     _previous;

    public SelectedProperties(Func<T, object> select) : this(select, Properties.Default) {}

    public SelectedProperties(Func<T, object> select, IProperties previous)
    {
        _select   = select;
        _previous = previous;
    }

    public IEnumerable<KeyValuePair<string, string?>> Get(object parameter)
    {
        foreach (var previous in _previous.Get(parameter))
        {
            yield return previous;
        }

        var from = _select(parameter.To<T>());
        foreach (var next in _previous.Get(from))
        {
            yield return next;
        }
    }
}