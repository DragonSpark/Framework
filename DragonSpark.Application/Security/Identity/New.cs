using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Identity;
using System;

namespace DragonSpark.Application.Security.Identity
{
	public class New<T> : ISelect<ExternalLoginInfo, T> where T : IdentityUser, new()
	{
		public static New<T> Default { get; } = new New<T>();

		New() : this(UniqueId.Default.Get) {}

		readonly Func<ExternalLoginInfo, string> _identifier, _name;

		public New(Func<ExternalLoginInfo, string> identifier) : this(identifier, identifier) {}

		public New(Func<ExternalLoginInfo, string> identifier, Func<ExternalLoginInfo, string> name)
		{
			_identifier = identifier;
			_name       = name;
		}

		public T Get(ExternalLoginInfo parameter)
		{
			var result = new T
			{
				Id       = _identifier(parameter),
				UserName = _name(parameter)
			};
			return result;
		}
	}
}