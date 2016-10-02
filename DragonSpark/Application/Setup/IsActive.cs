using System;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;

namespace DragonSpark.Application.Setup
{
	public sealed class IsActive : DecoratedSourceCache<Type, bool>
	{
		public static IParameterizedSource<IServiceProvider, IsActive> Default { get; } = new Cache<IServiceProvider, IsActive>( provider => new IsActive() );
		IsActive() : base( new ThreadLocalSourceCache<Type, bool>() ) {}
	}
}