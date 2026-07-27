using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;

namespace DragonSpark.Grok.Chat;

public class ToolRegistration<T> : Instance<Tool>, IToolRegistration
{
    readonly ISelect<IReadOnlyDictionary<string, object>, T> _map;
    readonly IExecute<T>                                     _execute;

    protected ToolRegistration(string name, IExecute<T> execute) : this(name, MapToObject<T>.Default, execute) {}

    protected ToolRegistration(string name, ISelect<IReadOnlyDictionary<string, object>, T> map, IExecute<T> execute)
        : base(new(new(name, string.Empty, FunctionParameters<T>.Default.Get())))
    {
        _map     = map;
        _execute = execute;
    }

    public ValueTask<string> Get(Stop<IReadOnlyDictionary<string, object>> parameter)
    {
        var (subject, stop) = parameter;
        var input = _map.Get(subject);
        return _execute.Get(new(input, stop));
    }
}