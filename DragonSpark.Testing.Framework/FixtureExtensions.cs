using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using System;

namespace DragonSpark.Testing.Framework
{
	public static class FixtureExtensions
	{
		public static T Create<T>( this IFixture @this, Type type ) => (T)new SpecimenContext( @this ).Resolve( type );
	}
}