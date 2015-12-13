using DragonSpark.Extensions;
using System;
using System.Linq;

namespace DragonSpark.Activation.FactoryModel
{
	public class FactoryParameterCoercer<TParameter> : IFactoryParameterCoercer<TParameter>
	{
		readonly IActivator activator;
		
		public FactoryParameterCoercer() : this( Activator.Current )
		{}

		public FactoryParameterCoercer( IActivator activator )
		{
			this.activator = activator;
		}

		public TParameter Coerce( object context )
		{
			var result = context is TParameter ? (TParameter)context : PerformCoercion( context );
			return result;
		}

		protected virtual TParameter PerformCoercion( object context )
		{
			var result = context.With( Construct, activator.Activate<TParameter> );
			return result;
		}

		protected TParameter Construct( object parameter )
		{
			var constructor = typeof(TParameter).Adapt().FindConstructor( parameter.GetType() );
			var result = (TParameter)constructor.With( info =>
			{
				var parameters = info.GetParameters().First().ParameterType.Adapt().Qualify( parameter ).Append( Enumerable.Repeat( Type.Missing, Math.Max( 0, constructor.GetParameters().Length - 1 ) ) ).ToArray();
				return info.Invoke( parameters );
			} );
			// var result = construct != null ? construct : parameter.AsTo<Type, TParameter>( activator.Activate<TParameter> );
			return result;
		}
	}
}