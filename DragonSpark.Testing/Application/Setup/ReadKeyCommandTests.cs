using DragonSpark.Application.Setup;
using DragonSpark.Testing.Framework.Application;
using Moq;
using Xunit;

namespace DragonSpark.Testing.Application.Setup
{
	public class ReadKeyCommandTests
	{
		[Theory, AutoData]
		public void Verify( Mock<IInputOutput> io, string message, string exit )
		{
			io.Setup( x => x.Reader.ReadLine() ).Verifiable();
			io.Setup( x => x.Writer.WriteLine() ).Verifiable();
			new ReadKeyCommand { Message = message, Exiting = exit }.Execute( io.Object );
			io.Verify( x => x.Writer.WriteLine(), Times.Once );
			io.Verify( x => x.Writer.Write( It.Is<string>( s => s.Contains( message ) ) ), Times.Once );
			io.Verify( x => x.Reader.ReadLine(), Times.Once );
			io.Verify( x => x.Writer.Write( It.Is<string>( s => s.Contains( exit ) ) ), Times.Once );
		}
	}
}