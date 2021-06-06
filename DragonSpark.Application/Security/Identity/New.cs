using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime;
using Microsoft.AspNetCore.Identity;
using System;

namespace DragonSpark.Application.Security.Identity
{
	public interface INew<out T> : ISelect<ExternalLoginInfo, T> where T : IdentityUser {}

	public sealed class New<T> : INew<T> where T : IdentityUser, new()
	{
		public static New<T> Default { get; } = new New<T>();

		New() : this(IdentifyingText.Default.Then().Accept<ExternalLoginInfo>(), Time.Default) {}

		readonly Func<ExternalLoginInfo, string> _name;
		readonly ITime                           _time;

		public New(Func<ExternalLoginInfo, string> name, ITime time)
		{
			_name = name;
			_time = time;
		}

		public T Get(ExternalLoginInfo parameter)
		{
			var result = new T
			{
				UserName = _name(parameter),
				Created  = _time.Get()
			};
			return result;
		}
	}
}