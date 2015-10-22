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
			Assert.Equal( new[]{ typeof(Derived), typeof(Class), typeof(object) }, typeof(Derived).GetHierarchy().ToArray() );
			Assert.Equal( new[]{ typeof(Derived), typeof(Class) }, typeof(Derived).GetHierarchy( false ).ToArray() );
		}

		[Fact]
		public void GetAllInterfaces()
		{
			var interfaces = typeof(Derived).GetAllInterfaces().OrderBy( x => x.Name ).ToArray();
			Assert.Equal( new[]{ typeof(IAnotherInterface), typeof(IInterface) }, interfaces );
		}

		[Fact]
		public void GetItemType()
		{
			Assert.Equal( typeof(Class), typeof(List<Class>).GetInnerType() );
			Assert.Equal( typeof(Class), typeof(Class[]).GetInnerType() );
			Assert.Null( typeof(Class).GetInnerType() );
		}
	}
}