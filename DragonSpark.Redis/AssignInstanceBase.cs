using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Redis;

class AssignInstanceBase<T> : ICommand<(T, IHostEnvironment)>
{
	readonly Func<T, string?>  _get;
	readonly Action<T, string> _set;

	protected AssignInstanceBase(Func<T, string?> get, Action<T, string> set)
	{
		_get = get;
		_set = set;
	}

	public void Execute((T, IHostEnvironment) parameter)
	{
		var (options, environment) = parameter;
		var qualifier = _get(options).NullIfEmpty() is {} value ? $"{value}-" : string.Empty;
		var instance  = $"{environment.EnvironmentName}>{qualifier}";
		_set(options, instance);
	}
}