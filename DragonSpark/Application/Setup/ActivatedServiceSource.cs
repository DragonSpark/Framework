using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.Specifications;
using System;

namespace DragonSpark.Application.Setup
{
	public sealed class ActivatedServiceSource : DecoratedParameterizedSource<Type, object>
	{
		readonly static DelegatedSpecification<object> Services = new DelegatedSpecification<object>( ServicesEnabled.Default.ToDelegate().Wrap() );

		public ActivatedServiceSource( IServiceProvider provider ) : this( provider, IsActive.Default.Get( provider ) ) {}
		ActivatedServiceSource( IServiceProvider provider, IsActive active ) : base( new Inner( provider, active ).Apply( Services.And( new DelegatedSpecification<Type>( active.Get ).Inverse() ) ) ) {}

		sealed class Inner : ParameterizedSourceBase<Type, object>
		{
			readonly IServiceProvider provider;
			readonly IsActive active;

			public Inner( IServiceProvider provider, IsActive active )
			{
				this.provider = provider;
				this.active = active;
			}

			public override object Get( Type parameter )
			{
				using ( active.Assignment( parameter, true ) )
				{
					var service = provider.GetService( parameter );
					return service;
				}
			}
		}
	}
}