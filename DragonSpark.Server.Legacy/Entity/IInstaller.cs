using System;
using System.Collections.Generic;

namespace DragonSpark.Server.Legacy.Entity
{
	[InheritedExport]
	public interface IInstaller
	{
		Guid Id { get; }

		Version Version { get; }

		Type ContextType { get; }

		IEnumerable<IInstallationStep> Steps { get; }
	}
}