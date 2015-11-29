using DragonSpark.Runtime;
using Ploeh.AutoFixture;
using System;

namespace DragonSpark.Testing.Framework
{
	public class CurrentTaskSpecification : ProvidedSpecification
	{
		public CurrentTaskSpecification() : this( new TaskLocalValue( new object() ) )
		{}

		public CurrentTaskSpecification( TaskLocalValue local ) : base( () => local.Item != null )
		{}
	}

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