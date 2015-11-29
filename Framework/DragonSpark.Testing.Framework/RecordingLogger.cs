using DragonSpark.Runtime;
using Ploeh.AutoFixture;
using System;
using DragonSpark.Runtime.Values;
using DragonSpark.Testing.Framework.Setup;

namespace DragonSpark.Testing.Framework
{
	public static class FixtureContext
	{
		public static IDisposable Create( IFixture item )
		{
			var key = new AmbientKey<IFixture>( new CurrentTaskSpecification() );
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