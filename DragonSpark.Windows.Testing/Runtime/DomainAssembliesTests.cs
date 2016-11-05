using DragonSpark.Windows.Runtime;
using System;
using System.IO;
using System.Reflection;
using Xunit;

namespace DragonSpark.Windows.Testing.Runtime
{
	public class DomainAssembliesTests
	{
		[Fact]
		public void Coverage()
		{
			DomainAssemblies.AssemblyLoader.Implementation.Assign( () =>
																   {
																	   throw new FileNotFoundException();
																   } );

			var result = new DomainAssemblies( DomainAssemblies.AssemblyLoader.Implementation.Get ).Get( AppDomain.CurrentDomain );
			Assert.Same( Assembly.GetEntryAssembly(), result );
		}
	}
}