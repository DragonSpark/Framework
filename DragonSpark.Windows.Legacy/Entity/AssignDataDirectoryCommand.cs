using DragonSpark.Commands;
using DragonSpark.ComponentModel;
using PostSharp.Patterns.Contracts;
using System.IO.Abstractions;

namespace DragonSpark.Windows.Legacy.Entity
{
	public class AssignDataDirectoryCommand : CommandBase<object>
	{
		[Singleton( typeof(EntityFiles), nameof(EntityFiles.DefaultDataDirectory) ), Required]
		public DirectoryInfoBase Directory { [return: Required]get; set; }

		[Service, Required]
		public DataDirectoryPath Path { [return: Required]get; set; }

		public override void Execute( object parameter ) => Path.Assign( Directory.FullName );
	}
}