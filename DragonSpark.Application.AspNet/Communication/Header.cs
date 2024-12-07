using DragonSpark.Model;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace DragonSpark.Application.Communication;

public class Header : IHeader
{
	readonly string _name;

	protected Header(string name) => _name = name;

	public string? Get(IHeaderDictionary parameter)
		=> parameter.TryGetValue(_name, out var value) ? value.ToString() : null;

	public void Execute(Pair<IHeaderDictionary, string> parameter)
	{
		var (key, value) = parameter;
		key[_name]       = value;
	}

	public string? Get(HttpHeaders parameter) => parameter.TryGetValues(_name, out var value) ? string.Join(";", value) : null;

	public void Execute(Pair<HttpHeaders, string> parameter)
	{
		var (key, value) = parameter;
		key.Add(_name, value);
	}
}