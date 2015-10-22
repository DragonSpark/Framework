using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Markup;

namespace DragonSpark.Windows.Entity
{
	[ContentProperty( "Steps" )]
	public abstract class Installer : IInstaller
	{
		public Guid Id { get; set; }

		[TypeConverter( typeof(VersionConverter) )]
		public Version Version { get; set; }

		public Type ContextType { get; set; }

		IEnumerable<IInstallationStep> IInstaller.Steps => Steps;

		public Collection<IInstallationStep> Steps { get; } = new Collection<IInstallationStep>();
	}
}