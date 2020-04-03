using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Runtime.Invocation.Expressions;
using System;
using System.Linq.Expressions;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Profile
{
	public class Accessor<T> : Condition<Claim>, IAccessor<T> where T : IdentityUser
	{
		readonly Action<T, string> _assign;
		readonly Func<T, string>   _get;

		protected Accessor(Action<T, string> assign, Expression<Func<T, string>> get, string type)
			: this(assign, get.Compile(), get.GetMemberInfo().Name, type)
		{}

		// ReSharper disable once TooManyDependencies
		protected Accessor(Action<T, string> assign, Func<T, string> get, string name, string type)
			: base(Start.A.Selection<Claim>().By.Calling(x => x.Type).Select(Is.EqualTo(type)))
		{
			_assign = assign;
			_get    = get;
			Name = name;
		}

		public void Execute((T User, string Value) parameter)
		{
			_assign(parameter.User, parameter.Value);
		}

		public string Get(T parameter) => _get(parameter);

		public string Name { get; }
	}
}