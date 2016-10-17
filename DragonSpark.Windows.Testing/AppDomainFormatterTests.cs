using DragonSpark.Application;
using DragonSpark.Testing.Framework.Application;
using DragonSpark.Testing.Framework.Application.Setup;
using System;
using Xunit;

namespace DragonSpark.Windows.Testing
{
	public class AppDomainFormatterTests
	{
		[Theory, AutoData, ContainingTypeAndNested]
		public void Verify() => 
			Assert.Equal( DefaultAssemblyInformationSource.Default.Get().Title, new AppDomainFormatter( AppDomain.CurrentDomain ).ToString() );
	}
}