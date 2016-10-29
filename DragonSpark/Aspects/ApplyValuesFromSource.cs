using DragonSpark.Activation.Location;
using DragonSpark.Application.Setup;
using DragonSpark.Extensions;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Aspects.Configuration;
using PostSharp.Aspects.Serialization;
using System;

namespace DragonSpark.Aspects
{
	[AspectConfiguration( SerializerType = typeof(MsilAspectSerializer) )]
	public sealed class ApplyValuesFromSource : InstanceLevelAspect
	{
		readonly Func<IServiceRepository> repositorySource;

		public ApplyValuesFromSource() : this( GlobalServiceProvider.Default.Get<IServiceRepository> ) {}

		public ApplyValuesFromSource( Func<IServiceRepository> repositorySource )
		{
			this.repositorySource = repositorySource;
		}

		[OnInstanceConstructedAdvice]
		public void OnInstanceConstructed()
		{
			var serviceType = Instance.GetType();
			var repository = repositorySource();
			if ( repository.IsSatisfiedBy( serviceType ) )
			{
				var source = repository.GetService( serviceType );
				source.MapInto( Instance );
			}
		}
	}
}
