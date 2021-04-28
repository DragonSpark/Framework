using DragonSpark.Model.Selection.Alterations;
using System;
using System.Linq.Dynamic.Core.Exceptions;

namespace DragonSpark.Presentation.Components.Diagnostics
{
	sealed class ExceptionCompensations : IAlteration<Exception>
	{
		public static ExceptionCompensations Default { get; } = new ExceptionCompensations();

		ExceptionCompensations() {}

		public Exception Get(Exception parameter) => parameter switch
		{
			ParseException parse => new InvalidOperationException($"{parse.Message}: {parse}", parse),
			_ => parameter
		};
	}
}