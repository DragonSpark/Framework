using System;
using DragonSpark.Server.Legacy.Io;
using Microsoft.Practices.Unity;

namespace DragonSpark.Server.Legacy.IoC.Io
{
	public class WorkspaceFactory : FactoryBase
	{
		public string WorkspacePath { get; set; }

		protected override object Create( IUnityContainer container, Type type, string buildName )
		{
			var result = Workspace.Create( WorkspacePath );
			return result;
		}
	}
}