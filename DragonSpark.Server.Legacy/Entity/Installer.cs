using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Markup;

namespace DragonSpark.Server.Legacy.Entity
{
	[ContentProperty( "Steps" )]
	public abstract class Installer : IInstaller
	{
		public Guid Id { get; set; }

		[TypeConverter( typeof(VersionConverter) )]
		public Version Version { get; set; }

		public Type ContextType { get; set; }

		IEnumerable<IInstallationStep> IInstaller.Steps
		{
			get { return Steps; }
		}

		public Collection<IInstallationStep> Steps
		{
			get { return steps ?? ( steps = new Collection<IInstallationStep>() ); }
		}	Collection<IInstallationStep> steps;
	}
}