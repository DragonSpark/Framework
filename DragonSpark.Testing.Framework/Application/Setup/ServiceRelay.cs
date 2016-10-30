using DragonSpark.Extensions;
using DragonSpark.TypeSystem;
using JetBrains.Annotations;
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

		[UsedImplicitly]
		public ServiceRelay( Func<Type, object> provider )
		{
			this.provider = provider;
		}

		public object Create( object request, ISpecimenContext context )
		{
			var result = TypeCoercer.Default.Coerce( request ).With( provider ) ?? new NoSpecimen();
			return result;
		}
	}
}