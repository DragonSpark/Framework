using DragonSpark.Application;
using DragonSpark.Extensions;
using DragonSpark.Testing.Framework;
using DragonSpark.Windows.Runtime;
using System.Linq;
using Xunit;

namespace DragonSpark.Windows.Testing.Runtime
{
	public class FileSystemTypesTests
	{
		[Theory, Ploeh.AutoFixture.Xunit2.AutoData, Trait( Traits.Category, Traits.Categories.FileSystem )]
		public void Assemblies( FileSystemTypes sut )
		{
			Assert.Same( sut, FileSystemTypes.Default );
			var assemblies = sut.Get().AsEnumerable().Assemblies();
			var specification = ApplicationAssemblySpecification.Default;

			Assert.True( assemblies.All( specification.IsSatisfiedBy ) );
		} 
	}
}