using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Security.Identity;

sealed class GetStore<T> : ISelect<UserManager<T>, DbContext> where T : class
{
	public static GetStore<T> Default { get; } = new();

	GetStore() : this(UserManagerStore<T>.Default) {}

	readonly UserManagerStore<T> _access;

	public GetStore(UserManagerStore<T> access) => _access = access;

	public DbContext Get(UserManager<T> parameter)
	{
		var instance = _access.Get(parameter);
		var result   = instance.GetType().GetProperty("Context").Verify().GetValue(instance).Verify().To<DbContext>();
		return result;
	}
}