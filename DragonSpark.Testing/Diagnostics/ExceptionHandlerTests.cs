using DragonSpark.Diagnostics;
using DragonSpark.Testing.Framework.Setup;
using System;
using Xunit;

namespace DragonSpark.Testing.Diagnostics
{
	public class ExceptionHandlerTests
	{
		[Theory, MoqAutoData]
		void Process( ExceptionHandler sut, Exception error )
		{
			Assert.Throws<Exception>( () => sut.Process( error ) );
		}
	}
}