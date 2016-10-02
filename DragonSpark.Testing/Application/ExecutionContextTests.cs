using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Application;
using DragonSpark.Testing.Framework.Runtime;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;

namespace DragonSpark.Testing.Application
{
	[Trait( Traits.Category, Traits.Categories.ServiceLocation )]
	public class ExecutionContextTests : TestCollectionBase
	{
		public ExecutionContextTests( ITestOutputHelper output ) : base( output ) {}

		public static void Verify( MethodBase method )
		{
			var current = MethodContext.Default.Get();
			if ( method != null && current != method )
			{
				throw new InvalidOperationException( $"Assigned Method is different from expected.  Expected: {method}.  Actual: {current}" );
			}
		}

		[Fact]
		public void Fact()
		{
			Assert.Equal( Identification.Default.Get(), Identifier.Current() );
			Assert.Null( MethodContext.Default.Get() );
		}

		[Theory, ExecutionContextAutoData]
		public void Theory()
		{
			var method = new Action( Theory ).Method;
			Verify( method );
			Assert.Equal( ExecutionContext.Default.Get().Origin, Identifier.Current() );
			Assert.Equal( method, MethodContext.Default.Get() );
		}
	}

	sealed class ExecutionContextAutoData : AutoDataAttribute
	{
		public override IEnumerable<object[]> GetData( MethodInfo methodUnderTest )
		{
			var enumerable = base.GetData( methodUnderTest );
			ExecutionContextTests.Verify( methodUnderTest );
			return enumerable;
		}
	}
}