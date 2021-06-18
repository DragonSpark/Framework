using DragonSpark.Compose;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DragonSpark.Application.Security.Identity.Model
{
	sealed class ReturnOrRootDefinition : IPagePathDefinition
	{
		public static ReturnOrRootDefinition Default { get; } = new();

		ReturnOrRootDefinition() : this(ReturnUrl.Default, "~/") {}

		readonly string _name;
		readonly string _root;

		public ReturnOrRootDefinition(string name, string root)
		{
			_name = name;
			_root = root;
		}

		public string Get((IUrlHelper Url, IValueProvider Values) parameter)
		{
			var (url, values) = parameter;
			var result = values.Get(_name) ?? url.Content(_root).Verify();
			return result;
		}
	}
}