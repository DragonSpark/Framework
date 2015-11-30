using System.Linq;
using DragonSpark.Extensions;

namespace DragonSpark.Activation.FactoryModel
{
	public class FactoryParameterQualifier<TParameter> : IFactoryParameterQualifier<TParameter>
	{
		readonly IActivator activator;
		
		public FactoryParameterQualifier() : this( Activator.Current )
		{}

		public FactoryParameterQualifier( IActivator activator )
		{
			this.activator = activator;
		}

		public TParameter Qualify( object context )
		{
			var result = context is TParameter ? (TParameter)context : context.With( Construct, activator.Activate<TParameter> );
			return result;
		}

		protected virtual TParameter Construct( object parameter )
		{
			var result = (TParameter)typeof(TParameter).Extend().FindConstructor( parameter.GetType() ).With( info => info.Invoke( new[] { info.GetParameters().First().ParameterType.Extend().Qualify( parameter ) } ) );
			return result;
		}
	}
}