using DragonSpark.Application.Runtime;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Runtime;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Profile
{
	public interface INew<T> : ISelecting<ExternalLoginInfo, T> where T : IdentityUser {}

	public sealed class New<T> : INew<T> where T : IdentityUser
	{
		public static New<T> Default { get; } = new New<T>();

		New() : this(Start.A.Selection<ExternalLoginInfo>().By.Instantiation<T>(),
		             IdentifyingText.Default.Then().Accept<ExternalLoginInfo>(), Time.Default) {}

		readonly Func<ExternalLoginInfo, T>      _new;
		readonly Func<ExternalLoginInfo, string> _name;
		readonly ITime                           _time;

		public New(Func<ExternalLoginInfo, T> @new, Func<ExternalLoginInfo, string> name, ITime time)
		{
			_new  = @new;
			_name = name;
			_time = time;
		}

		public ValueTask<T> Get(ExternalLoginInfo parameter)
		{
			var user = _new(parameter);
			user.UserName = _name(parameter);
			user.Created  = _time.Get();

			var result = user.ToOperation();
			return result;
		}
	}
}