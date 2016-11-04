using DragonSpark.Sources.Coercion;
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
		ServiceRelay() : this( Defaults.ServiceSource.Accept( TypeCoercer.Default ).Get ) {}

		readonly Func<object, object> provider;

		[UsedImplicitly]
		public ServiceRelay( Func<object, object> provider )
		{
			this.provider = provider;
		}

		public object Create( object request, ISpecimenContext context ) => provider( request ) ?? new NoSpecimen();
	}
}