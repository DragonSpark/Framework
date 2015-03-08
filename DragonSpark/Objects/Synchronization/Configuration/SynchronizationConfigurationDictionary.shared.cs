using System;
using System.Reflection;
using DragonSpark.Configuration;
using DragonSpark.Objects;

namespace DragonSpark.Objects.Synchronization.Configuration
{
	public class SynchronizationConfigurationDictionary : ConfigurationDictionary<Synchronization.SynchronizationContainer>, IFactory
	{
		readonly static MethodInfo CreateMethod = typeof(SynchronizationConfigurationDictionary).GetMethod( "Create", new[] { typeof(object) } );

		public SynchronizationConfigurationDictionary()
		{
			IncludeBasePolicies = true;
		}

		public bool IncludeBasePolicies { get; set; }

		public TResultType Create<TResultType>( object source ) where TResultType : class
		{
			var creator = Locator.GetInstance<IFactory>() ?? new Factory<TResultType>();
			var result = (TResultType)creator.Create( typeof(TResultType), source );
			Synchronize( source, result );
			return result;
		}

		public void Synchronize<TSourceType,TResultType>( TSourceType source, TResultType target )
		{
			Synchronize( source, target, null );
		}

		public void Synchronize<TSourceType,TResultType>( TSourceType source, TResultType target, string name )
		{
			Instance.Synchronize( source, target, name, IncludeBasePolicies );
		}

		object IFactory.Create( Type resultType, object source )
		{
			var result = CreateMethod.MakeGenericMethod( resultType ).Invoke( this, new[] { source } );
			return result;
		}
	}
}
