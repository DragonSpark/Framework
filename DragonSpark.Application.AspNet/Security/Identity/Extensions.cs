using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Security.Identity;

public static class Extensions
{
	public static ValueTask<UserManager<T>> Attached<T>(this UserManager<T> @this, DbContext existing) where T : class
		=> AttachContext<T>.Default.Get(new(@this, existing));
}