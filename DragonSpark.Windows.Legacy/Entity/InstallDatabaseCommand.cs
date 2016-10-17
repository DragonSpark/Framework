using DragonSpark.Commands;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Windows.Legacy.Properties;
using JetBrains.Annotations;
using System.IO;
using System.Linq;

namespace DragonSpark.Windows.Legacy.Entity
{
	public class InstallDatabaseCommand : RunCommandBase
	{
		readonly static byte[][] Data = { Resources.Blank, Resources.Blank_log };

		[Service, PostSharp.Patterns.Contracts.NotNull, UsedImplicitly]
		public FileInfo Database { [return: PostSharp.Patterns.Contracts.NotNull]get; set; }

		public override void Execute()
		{
			if ( !Database.Exists )
			{
				foreach ( var item in EntityFiles.WithLog( Database ).Tuple( Data ).ToArray() )
				{
					var fullName = item.Item1.FullName;
					var directoryRoot = Path.GetDirectoryName( fullName );
					if ( directoryRoot != null )
					{
						Directory.CreateDirectory( directoryRoot );
						using ( var stream = File.Create( fullName ) )
						{
							stream.Write( item.Item2, 0, item.Item2.Length );
						}
					}
				}
			}
		}
	}
}