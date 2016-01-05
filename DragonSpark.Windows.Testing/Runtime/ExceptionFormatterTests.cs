using DragonSpark.Windows.Runtime;
using Ploeh.AutoFixture.Xunit2;
using System;
using Xunit;

namespace DragonSpark.Windows.Testing.Runtime
{
	public class ExceptionFormatterTests
	{
		[Theory, DragonSpark.Testing.Framework.Setup.AutoDataMoq]
		public void Format( [Frozen]Guid id, ExceptionFormatter sut, Exception exception )
		{
			var formatted = sut.Format( exception );
			Assert.Contains( exception.GetType().FullName, formatted );
			Assert.Contains( id.ToString(), formatted );
		} 
	}
}