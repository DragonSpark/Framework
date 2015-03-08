using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity;

namespace DragonSpark.Application.Communication.Entity
{
	public interface IInstallationStep
	{
		void Execute( DbContext context );
	}



	[InheritedExport]
	public interface IInstaller
	{
		Guid Id { get; }

		Version Version { get; }

		Type ContextType { get; }

		IEnumerable<IInstallationStep> Steps { get; }
	}
}