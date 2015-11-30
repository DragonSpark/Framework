using System;
using DragonSpark.Extensions;

namespace DragonSpark.Activation.FactoryModel
{
	public class ConstructFactoryParameterQualifier<TResult> : FactoryParameterQualifier<ConstructParameter>
	{
		public ConstructFactoryParameterQualifier()
		{}

		public ConstructFactoryParameterQualifier( IActivator activator ) : base( activator )
		{}

		protected override ConstructParameter Construct( object parameter )
		{
			var result = parameter.AsTo<Type, ConstructParameter>( type => typeof(TResult).Extend().IsAssignableFrom( type ) ? base.Construct( parameter ) : null ) ?? new ConstructParameter( typeof(TResult), parameter );
			return result;
		}
	}
}