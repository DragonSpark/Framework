using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Identity;
using System;

namespace DragonSpark.Application.Security.Identity
{
	public class New<T> : ISelect<ExternalLoginInfo, T> where T : IdentityUser, new()
	{
		public static New<T> Default { get; } = new New<T>();

		New() : this(UniqueId.Default.Get) {}

		readonly Func<ExternalLoginInfo, string> _name;

		public New(Func<ExternalLoginInfo, string> name) => _name = name;

		public T Get(ExternalLoginInfo parameter)
		{
			var result = new T
			{
				UserName = _name(parameter)
			};
			return result;
		}
	}
}