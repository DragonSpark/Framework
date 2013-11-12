using System;
using System.IO;
using System.Reflection;

namespace DragonSpark.Server.ClientHosting
{
	[AttributeUsage( AttributeTargets.Assembly, AllowMultiple = true )]
	public class ClientResourceAttribute : Attribute
	{
		public ClientResourceAttribute( string fileName )
		{
			FileName = fileName;
		}

		public string FileName { get; private set; }

		public Priority Priority { get; set; }

		public string GetName( Assembly assembly )
		{
			var result = string.Concat( assembly.GetName().Name, '.', FileName.Replace( Path.AltDirectorySeparatorChar, '.' ) );
			return result;
		}
	}
}