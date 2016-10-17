using System;
using System.Collections.Generic;

namespace DragonSpark.Windows.Legacy.Entity
{
	public interface IInstaller
	{
		Guid Id { get; }

		Version Version { get; }

		Type ContextType { get; }

		IEnumerable<IInstallationStep> Steps { get; }
	}
}