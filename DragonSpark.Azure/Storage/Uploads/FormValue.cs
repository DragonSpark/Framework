using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Azure.Storage.Uploads;

class FormValue<T> : ISelect<IFormCollection, T?> where T : struct
{
	readonly string          _key;
	readonly Func<string, T> _select;

	protected FormValue(string key, Func<string, T> select)
	{
		_key    = key;
		_select = select;
	}

	public T? Get(IFormCollection parameter)
		=> parameter.TryGetValue(_key, out var text) ? _select(text.ToString()) : default;
}