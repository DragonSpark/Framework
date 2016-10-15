using JetBrains.Annotations;
using System;

namespace DragonSpark.Composition
{
	public class ExportMapping<TSubject, TExport> : ExportMapping where TSubject : TExport
	{
		public ExportMapping() : base( typeof(TSubject), typeof(TExport) ) {}
	}

	public class ExportMapping
	{
		public ExportMapping( Type subject ) : this( subject, subject ) {}

		public ExportMapping( Type subject, Type exportAs )
		{
			Subject = subject;
			ExportAs = exportAs;
		}

		public Type Subject { [return: PostSharp.Patterns.Contracts.NotNull]get; set; }

		[UsedImplicitly]
		public Type ExportAs { get; set; }
	}
}