using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Profile {
	sealed class UserMapping<T> : IUserMapping where T : IdentityUser
	{
		readonly IAssignment<T> _assignment;

		public UserMapping(IAssignment<T> assignment, string key, bool required)
		{
			Key         = key;
			Required    = required;
			_assignment = assignment;
		}

		public string Key { get; }
		public bool Required { get; }

		public bool Get(Claim parameter) => _assignment.Get(parameter);

		public void Execute((IdentityUser User, string Value) parameter)
		{
			_assignment.Execute((parameter.User.To<T>(), parameter.Value));
		}
	}
}