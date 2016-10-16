using DragonSpark.Commands;
using System.Windows.Input;
using Xunit;

namespace DragonSpark.Testing.Commands
{
	public class ExtensionsTests
	{
		[Fact]
		public void ToSpecification()
		{
			var sut = new Command<object>();
			ICommand command = sut;
			Assert.NotSame( sut.ToSpecification(), command.ToSpecification() );
			// var sut = command.ToSpecification();
		}
	}
}