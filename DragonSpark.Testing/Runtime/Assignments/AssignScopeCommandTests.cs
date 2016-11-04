using DragonSpark.Application;
using DragonSpark.Runtime.Assignments;
using System;
using Xunit;

namespace DragonSpark.Testing.Runtime.Assignments
{
	public class AssignScopeCommandTests
	{
		[Fact]
		public void Coverage()
		{
			var now = DateTimeOffset.UtcNow;
			new AssignScopeCommand<DateTimeOffset>( Clock.Default ).Execute( () => now );
			Assert.Equal( Clock.Default.Get(), now );
		}
	}
}