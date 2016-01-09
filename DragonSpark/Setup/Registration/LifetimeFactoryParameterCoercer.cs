using System;
using DragonSpark.Activation;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Aspects;
using DragonSpark.Extensions;
using DragonSpark.TypeSystem;
using Microsoft.Practices.Unity;
using PostSharp.Patterns.Contracts;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.Setup.Registration
{
	public class LifetimeFactoryParameterCoercer : ActivateFactoryParameterCoercer<LifetimeManager>
	{
		readonly IAttributeProvider provider;
		readonly Type defaultLifetimeType;

		public LifetimeFactoryParameterCoercer( Type defaultLifetimeType ) : this( AttributeProvider.Instance, Activator.GetCurrent, defaultLifetimeType )
		{}

		public LifetimeFactoryParameterCoercer( [Required]IAttributeProvider provider, [Required]Func<IActivator> activator, [OfType( typeof(LifetimeManager) )]Type defaultLifetimeType ) : base( activator )
		{
			this.provider = provider;
			this.defaultLifetimeType = defaultLifetimeType;
		}

		protected override ActivateParameter Create( Type type, object parameter )
		{
			var lifetimeManagerType = provider.FromMetadata<LifetimeManagerAttribute, Type>( type, x => x.LifetimeManagerType ) ?? defaultLifetimeType;
			var result = base.Create( lifetimeManagerType, parameter );
			return result;
		}
	}
}