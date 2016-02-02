using DragonSpark.ComponentModel;
using DragonSpark.Setup;
using System.IO;
using DragonSpark.Aspects;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Windows.Entity
{
	public class AssignDataDirectoryCommand : SetupCommandBase
	{
		[Singleton( typeof(EntityFiles), nameof(EntityFiles.DefaultDataDirectory) ), Required]
		public DirectoryInfo Directory { [return: Required]get; set; }

		[Locate, Required]
		public DataDirectoryPath Path { [return: Required]get; set; }

		[BuildUp]
		protected override void OnExecute( object parameter ) => Path.Assign( Directory.FullName );
	}
}