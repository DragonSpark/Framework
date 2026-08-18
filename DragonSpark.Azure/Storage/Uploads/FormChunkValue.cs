using DragonSpark.Model.Selection;

namespace DragonSpark.Azure.Storage.Uploads;

class FormChunkValue<T> : ISelect<FormChunkValueInput, T?> where T : struct
{
	readonly string          _key;
	readonly Func<string, T> _select;

	protected FormChunkValue(string key, Func<string, T> select)
	{
		_key    = key;
		_select = select;
	}

	public T? Get(FormChunkValueInput parameter)
	{
		var (form, index) = parameter;

		if (form.TryGetValue(_key, out var text))
		{
			var content = text.ToString();
			var parts   = content.Split(',');
			if (index.HasValue && parts.Any())
			{
				var part  = parts[Math.Min(parts.Length-1, index.Value)];
				return _select(part);
			}
			
			return _select(content);
		}

		return null;
	}
}