using DragonSpark.Extensions;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Parameters;
using DragonSpark.Testing.Framework.Setup;
using DragonSpark.Testing.TestObjects;
using DragonSpark.TypeSystem;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Extensions
{
	public class AssemblyLocatorExtensionsTests
	{
		[Theory, Test, SetupAutoData]
		public void GetAllTypesWith( [Located]AssembliesFactory sut )
		{
			var items = sut.Create().GetAllTypesWith<PriorityAttribute>();
			Assert.True( items.Select( tuple => tuple.Item2 ).AsTypes().Contains( typeof(NormalPriority) ) );
		} 
	}
}