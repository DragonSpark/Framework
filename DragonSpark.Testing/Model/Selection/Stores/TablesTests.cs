using DragonSpark.Model.Selection.Stores;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Testing.Model.Selection.Stores
{
	public class TablesTests
	{
		[Fact]
		public void Verify()
		{
			Tables<string, object>.Default.Get(_ => null!)
			                      .Should()
			                      .NotBeSameAs(Tables<string, object>.Default.Get(_ => null!));
		}
	}
}