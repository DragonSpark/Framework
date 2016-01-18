using DragonSpark.ComponentModel;
using DragonSpark.Setup;
using System.IO;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Windows.Entity
{
	public class AssignDataDirectoryCommand : SetupCommand
	{
		[Singleton( typeof(EntityFiles), nameof(EntityFiles.DefaultDataDirectory) ), Required]
		public DirectoryInfo Directory { [return: Required]get; set; }

		[Locate, Required]
		public DataDirectoryPath Path { [return: Required]get; set; }

		protected override void OnExecute( ISetupParameter parameter ) => Path.Assign( Directory.FullName );
	}
}