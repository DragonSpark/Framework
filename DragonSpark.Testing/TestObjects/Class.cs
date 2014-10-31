using System;
using DragonSpark.Extensions;

namespace DragonSpark.Testing.TestObjects
{
	class Class : IInterface
	{}

	class ClassWithBrokenConstructor : IInterface
	{
		public ClassWithBrokenConstructor()
		{
			throw new InvalidOperationException( "This is a broken constructor" );
		}
	}
}