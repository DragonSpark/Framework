using DragonSpark.Testing.Framework.Setup;
using DragonSpark.Windows.Runtime;
using Ploeh.AutoFixture.Xunit2;
using System;
using Xunit;

namespace DragonSpark.Windows.Testing.Runtime
{
	public class ExceptionFormatterTests
	{
		[Theory, MoqAutoData]
		public void Format( [Frozen]Guid id, [Greedy]ExceptionFormatter sut, Exception exception )
		{
			var formatted = sut.Format( exception );
			Assert.Contains( exception.GetType().FullName, formatted );
			Assert.Contains( id.ToString(), formatted );
		} 
	}
}