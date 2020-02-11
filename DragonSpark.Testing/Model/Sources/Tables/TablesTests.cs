using DragonSpark.Model.Selection.Stores;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Testing.Model.Sources.Tables
{
	public class TablesTests
	{
		[Fact]
		void Verify()
		{
			Tables<string, object>.Default.Get(_ => null)
			                      .Should()
			                      .NotBeSameAs(Tables<string, object>.Default.Get(_ => null));
		}
	}
}