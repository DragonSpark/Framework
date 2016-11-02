using DragonSpark.Application;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Application;
using DragonSpark.Testing.Framework.FileSystem;
using DragonSpark.TypeSystem;
using DragonSpark.Windows.Runtime;
using System.Linq;
using Xunit;

namespace DragonSpark.Windows.Testing.Runtime
{
	public class FileSystemTypesTests
	{
		[Theory, AutoData, Trait( Traits.Category, Traits.Categories.FileSystem ), InitializeFileSystem]
		public void Assemblies( FileSystemTypes sut )
		{
			Assert.Same( sut, FileSystemTypes.Default );
			var assemblies = sut.Assemblies();
			var specification = RegisteredAssemblySpecification.Default;

			Assert.True( assemblies.All( specification.IsSatisfiedBy ) );
		} 
	}
}