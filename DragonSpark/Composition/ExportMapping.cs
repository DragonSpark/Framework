using DragonSpark.Extensions;
using PostSharp.Patterns.Contracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Composition
{
	public class ExportMapping<TSubject, TExport> : ExportMapping where TSubject : TExport
	{
		public ExportMapping() : base( typeof(TSubject), typeof(TExport) ) {}
	}

	public class ExportMapping : IEnumerable<Type>
	{
		public ExportMapping( Type subject ) : this( subject, subject ) {}

		public ExportMapping( Type subject, Type exportAs )
		{
			Subject = subject;
			ExportAs = exportAs;
		}

		public Type Subject { [return: NotNull]get; set; }

		public Type ExportAs { get; set; }

		public IEnumerator<Type> GetEnumerator() => Subject.Append( ExportAs ).WhereAssigned().Distinct().GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}