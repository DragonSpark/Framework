using DragonSpark.Extensions;
using DragonSpark.TypeSystem;
using Ploeh.AutoFixture.Kernel;
using System;
using Defaults = DragonSpark.Activation.Location.Defaults;

namespace DragonSpark.Testing.Framework.Application.Setup
{
	public sealed class ServiceRelay : ISpecimenBuilder
	{
		public static ServiceRelay Default { get; } = new ServiceRelay();
		ServiceRelay() : this( Defaults.ServiceSource ) {}

		readonly Func<Type, object> provider;

		public ServiceRelay( Func<Type, object> provider )
		{
			this.provider = provider;
		}

		public object Create( object request, ISpecimenContext context )
		{
			var type = TypeSupport.From( request );
			var result = type.With( provider ) ?? new NoSpecimen();
			return result;
		}
	}
}