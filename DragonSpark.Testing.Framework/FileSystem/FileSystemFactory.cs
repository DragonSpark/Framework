namespace DragonSpark.Testing.Framework.FileSystem
{
	/*public sealed class FileSystemFactory : ParameterizedSourceBase<IEnumerable<string>, DirectoryInfoBase>
	{
		public static FileSystemFactory Default { get; } = new FileSystemFactory();
		FileSystemFactory() : this( FileElement.Empty() ) {}

		readonly FileElement file;

		public FileSystemFactory( FileElement file )
		{
			this.file = file;
		}

		public override DirectoryInfoBase Get( IEnumerable<string> parameter )
		{
			/*var files = new Dictionary<string, FileElement>();
			foreach ( var path in parameter )
			{
				files.Add( path, file );
			}#1#
			var fileSystem = new FileSystem();
			var result = fileSystem.FromDirectoryName( "." );
			return result;
		}
	}*/


}
