using System;
using DragonSpark.Io;
using DragonSpark.IoC.Configuration;
using Microsoft.Practices.Unity;

namespace DragonSpark.IoC.Io
{
	public class WorkspaceFactory : FactoryBase
	{
		public string WorkspacePath { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "This is the factory method.  The result is disposed elsewhere." )]
		protected override object Create( IUnityContainer container, Type type, string buildName )
		{
			var result = Workspace.Create( WorkspacePath );
			return result;
		}
	}
}