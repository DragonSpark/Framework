namespace DragonSpark.Application.Communication;

sealed class ClientStateValue : IClientStateValue
{
	readonly IClientStateValues _values;
	readonly char               _separator;

	public ClientStateValue(IClientStateValues values, char separator = ';')
	{
		_values    = values;
		_separator = separator;
	}

	public string Get() => string.Join(_separator, _values.Get());
}