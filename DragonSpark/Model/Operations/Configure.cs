using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

sealed class Configure<T> : ISelect<ValueTask<T>, ValueTask<T>>
{
	readonly Action<T> _configure;
	readonly bool      _capture;

	public Configure(Action<T> configure, bool capture = false)
	{
		_configure = configure;
		_capture   = capture;
	}

	public async ValueTask<T> Get(ValueTask<T> parameter)
	{
		if (parameter.IsCompletedSuccessfully)
		{
			var result = parameter.Result;
			_configure(result);
			return result;
		}

		{
			var result = await parameter.ConfigureAwait(_capture);
			_configure(result);
			return result;
		}
	}
}