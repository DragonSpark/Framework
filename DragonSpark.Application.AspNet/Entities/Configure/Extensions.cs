using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DragonSpark.Application.AspNet.Entities.Configure;

public static class Extensions
{
	public static T Add<T>(this T @this, IDbContextOptionsExtension extension)
		where T : IRelationalDbContextOptionsBuilderInfrastructure
	{
		@this.OptionsBuilder.To<IDbContextOptionsBuilderInfrastructure>().AddOrUpdateExtension(extension);
		return @this;
	}
}