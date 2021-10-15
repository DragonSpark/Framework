using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

sealed class InvokingParameter<T> : ISelect<ValueTask<T>, ValueTask>
{
	readonly Action<T> _configure;
	readonly bool      _capture;

	public InvokingParameter(Action<T> configure, bool capture = false)
	{
		_configure = configure;
		_capture   = capture;
	}

	public async ValueTask Get(ValueTask<T> parameter)
	{
		var input = parameter.IsCompletedSuccessfully ? parameter.Result : await parameter.ConfigureAwait(_capture);
		_configure(input);
	}
}