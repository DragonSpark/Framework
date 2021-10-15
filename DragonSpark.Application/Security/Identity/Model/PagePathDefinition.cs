using DragonSpark.Compose;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DragonSpark.Application.Security.Identity.Model;

public class PagePathDefinition : IPagePathDefinition
{
	readonly string _path, _handler;
	readonly IValue _value;

	protected PagePathDefinition(string path, string handler, IValue value)
	{
		_path    = path;
		_handler = handler;
		_value   = value;
	}

	public string Get((IUrlHelper Url, IValueProvider Values) parameter)
	{
		var (url, values) = parameter;
		var result = url.Page(_path, _handler, _value.Get(values)).Verify();
		return result;
	}
}