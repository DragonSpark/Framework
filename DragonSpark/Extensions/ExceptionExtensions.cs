using System;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace DragonSpark.Extensions
{
	public static class ExceptionExtensions
	{
		public static void Throw( this Exception target )
		{
			if ( target != null )
			{
				throw target;
			}
		}

		public static string Formatted( this Exception @this, Guid? contextId = null )
		{
			var writer = new StringWriter();
			new TextExceptionFormatter( writer, @this, contextId.GetValueOrDefault() ).Format();
			var result = writer.GetStringBuilder().ToString();
			return result;
		}
	}
}