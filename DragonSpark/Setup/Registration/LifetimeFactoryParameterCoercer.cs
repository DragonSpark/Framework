using DragonSpark.Activation;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Aspects;
using DragonSpark.Extensions;
using Microsoft.Practices.Unity;
using PostSharp.Patterns.Contracts;
using System;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.Setup.Registration
{
	public class LifetimeFactoryParameterCoercer : ActivateFactoryParameterCoercer<LifetimeManager>
	{
		readonly Type defaultLifetimeType;

		public LifetimeFactoryParameterCoercer( Type defaultLifetimeType ) : this( Activator.GetCurrent, defaultLifetimeType ) {}

		public LifetimeFactoryParameterCoercer( [Required]Activator.Get activator, [OfType( typeof(LifetimeManager) )]Type defaultLifetimeType ) : base( activator )
		{
			this.defaultLifetimeType = defaultLifetimeType;
		}

		protected override ActivateParameter Create( Type type, object parameter )
		{
			var lifetimeManagerType = type.From<LifetimeManagerAttribute, Type>( x => x.LifetimeManagerType ) ?? defaultLifetimeType;
			var result = base.Create( lifetimeManagerType, parameter );
			return result;
		}
	}
}