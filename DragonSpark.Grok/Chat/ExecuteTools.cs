using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Grok.Chat;

sealed class ExecuteTools : IExecuteTools
{
    readonly IConditional<string, IToolRegistration> _registrations;

    public ExecuteTools(IEnumerable<IToolRegistration> registrations) : this(registrations.ToArray()) {}

    public ExecuteTools(IToolRegistration[] registrations)
        : this(registrations.ToDictionary(x => x.Get().Function.Name).ToTable()) {}

    public ExecuteTools(IConditional<string, IToolRegistration> registrations) => _registrations = registrations;

    public ValueTask<string> Get(Stop<ExecuteInput> parameter)
    {
        var ((name, arguments), stop) = parameter;
        return _registrations.TryGet(name, out var registration)
                   ? registration.Get(new(arguments, stop))
                   : $"An attempt was made to access tool {name}, but it does not exist".ToOperation();
    }
}