using DragonSpark.Compose;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Profile
{
	sealed class UserMapping<T> : IUserMapping where T : IdentityUser
	{
		readonly IAccessor<T> _accessor;

		public UserMapping(IAccessor<T> accessor, string key, bool required)
		{
			Key       = key;
			Required  = required;
			_accessor = accessor;
		}

		public string Key { get; }
		public bool Required { get; }

		public bool Get(Claim parameter) => _accessor.Get(parameter);

		public void Execute((IdentityUser User, string Value) parameter)
		{
			_accessor.Execute((parameter.User.To<T>(), parameter.Value));
		}

		public string? Get(IdentityUser parameter) => _accessor.Get(parameter.To<T>());

		public string Name => _accessor.Name;
	}
}