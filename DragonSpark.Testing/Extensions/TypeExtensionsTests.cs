using DragonSpark.Extensions;
using DragonSpark.Testing.TestObjects;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Extensions
{
	public class TypeExtensionsTests
	{
		[Fact]
		public void GetHierarchy()
		{
			Assert.Equal( new[]{ typeof(Derived), typeof(Class), typeof(object) }, typeof(Derived).Extend().GetHierarchy().ToArray() );
			Assert.Equal( new[]{ typeof(Derived), typeof(Class) }, typeof(Derived).Extend().GetHierarchy( false ).ToArray() );
		}

		[Fact]
		public void GetAllInterfaces()
		{
			var interfaces = typeof(Derived).Extend().GetAllInterfaces().OrderBy( x => x.Name ).ToArray();
			Assert.Equal( new[]{ typeof(IAnotherInterface), typeof(IInterface) }, interfaces );
		}

		[Fact]
		public void GetItemType()
		{
			Assert.Equal( typeof(Class), typeof(List<Class>).Extend().GetInnerType() );
			Assert.Equal( typeof(Class), typeof(Class[]).Extend().GetInnerType() );
			Assert.Null( typeof(Class).Extend().GetInnerType() );
		}
	}
}