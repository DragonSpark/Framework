using System;
using System.IO;
using System.Xaml;
using DragonSpark.Windows.Io;

namespace DevelopersWin.VoteReporter
{
	public interface IStorage
	{
		Uri Save( object item, string fileName = null );
	}

	class Storage : IStorage
	{
		readonly ISerializer serializer;
		readonly DirectoryInfo directory;

		public Storage( ISerializer serializer, DirectoryInfo directory )
		{
			this.serializer = serializer;
			this.directory = directory;
		}

		public Uri Save( object item, string fileName )
		{
			var extension = item is string ? "txt" : "xaml";
			var path = Path.Combine( directory.FullName, fileName ?? $"{FileSystem.GetValidPath()}.{extension}" );
			var content = item as string ?? serializer.Serialize( item );
			File.WriteAllText( path, content );

			var result = new Uri( path );
			return result;
		}
	}

	public interface ISerializer
	{
		string Serialize( object item );
	}

	class Serializer : ISerializer
	{
		public string Serialize( object item )
		{
			var result = XamlServices.Save( item );
			return result;
		}
	}
}