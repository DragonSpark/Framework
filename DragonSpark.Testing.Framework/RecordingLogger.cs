using DragonSpark.Activation;
using DragonSpark.Runtime;
using Ploeh.AutoFixture;
using System;

namespace DragonSpark.Testing.Framework
{
	public static class FixtureContext
	{
		public static IDisposable Create( IFixture item )
		{
			var local = new TaskLocalValue( item );
			var key = new AmbientKey<IFixture>( new ProvidedSpecification( () => local.Item != null ) );
			var result = new AmbientContext( key, item );
			return result;
		}

		public static IFixture GetCurrent()
		{
			var result = AmbientValues.Get<IFixture>();
			return result;
		}
	}
}