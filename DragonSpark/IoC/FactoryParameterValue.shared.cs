using System;
using System.Diagnostics.Contracts;
using DragonSpark.Objects;
using Microsoft.Practices.ObjectBuilder2;

namespace DragonSpark.IoC
{
	public class FactoryParameterValue : Microsoft.Practices.Unity.TypedInjectionValue
	{
		readonly IFactory factory;

		public FactoryParameterValue( IFactory factory, Type parameterType ) : base( parameterType )
		{
			Contract.Requires( factory != null );
			this.factory = factory;
		}

		/*[ContractInvariantMethod]
		void Invariant()
		{
			Contract.Invariant( factory != null );	
		}*/

		public override IDependencyResolverPolicy GetResolverPolicy( Type typeToBuild )
		{
			var result = new FactoryValueResolverPolicy( factory );
			return result;
		}

		class FactoryValueResolverPolicy : IDependencyResolverPolicy
		{
			readonly IFactory factory;
			readonly object source;

			public FactoryValueResolverPolicy( IFactory factory, object source = null )
			{
				Contract.Requires( factory != null );

				this.factory = factory;
				this.source = source;
			}

			/*[ContractInvariantMethod]
			void Invariant()
			{
				Contract.Invariant( factory != null );	
			}*/


			public object Resolve( IBuilderContext context )
			{
				var result = factory.Create( context.BuildKey.Type, source ?? context );
				return result;
			}
		}
	}
}