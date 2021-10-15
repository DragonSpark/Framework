using DragonSpark.Model.Selection.Stores;
using System;
using System.Diagnostics;

namespace DragonSpark.Diagnostics;

sealed class EnhancedExceptions : ReferenceValueTable<Exception, Exception>
{
	public static EnhancedExceptions Default { get; } = new EnhancedExceptions();

	EnhancedExceptions() : base(x => x.Demystify()) {}
}