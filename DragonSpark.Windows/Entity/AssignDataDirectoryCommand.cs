using DragonSpark.ComponentModel;
using DragonSpark.Setup;
using System.IO;

namespace DragonSpark.Windows.Entity
{
	public class AssignDataDirectoryCommand : SetupCommand
	{
		[ComponentModel.Singleton( typeof(EntityFiles), nameof(EntityFiles.DefaultDataDirectory) )]
		public DirectoryInfo Directory { get; set; }

		[Activate]
		public DataDirectoryPath Path { get; set; }

		protected override void OnExecute( ISetupParameter parameter )
		{
			Path.Assign( Directory.FullName );
		}
	}
}