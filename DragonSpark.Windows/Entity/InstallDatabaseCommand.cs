using DragonSpark.Commands;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Windows.Properties;
using PostSharp.Patterns.Contracts;
using System.IO;
using System.Linq;

namespace DragonSpark.Windows.Entity
{
	public class InstallDatabaseCommand : CommandBase<object>
	{
		readonly static byte[][] Data = { Resources.Blank, Resources.Blank_log };

		[Service, Required]
		public FileInfo Database { [return: NotNull]get; set; }

		public override void Execute( object parameter )
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