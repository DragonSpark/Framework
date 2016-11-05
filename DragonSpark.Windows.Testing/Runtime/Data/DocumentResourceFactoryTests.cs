using DragonSpark.Windows.Runtime.Data;
using System;
using Xunit;

namespace DragonSpark.Windows.Testing.Runtime.Data
{
	public class DocumentResourceFactoryTests
	{
		[Fact]
		public void Verify() => 
			Assert.Throws<System.Net.WebException>( () => DocumentResourceFactory.Default.Get( new Uri( "http://does.not/exist/document.xml" ) ) );
	}
}