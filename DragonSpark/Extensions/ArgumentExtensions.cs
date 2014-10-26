using DragonSpark.Properties;
using System;

namespace DragonSpark.Extensions
{
	public static class ArgumentExtensions
	{
		public static string EnsureNotEmpty( this string target )
		{
			if ( string.IsNullOrEmpty( target ) )
			{
				throw new ArgumentException( Resources.Message_NullOrEmpty, "target" );
			}
			return target;
		}

		public static TItem EnsureOfType<TItem>( this TItem target, Type type ) where TItem : class
		{
			var result = target.Ensure<TItem>();
			var resultType = result.GetType();
			if ( !type.IsAssignableFrom( resultType ) )
			{
				throw new ArgumentException( string.Format( "'{0}' does not derrive from type '{1}'.", resultType.AssemblyQualifiedName, type.AssemblyQualifiedName ), "target" );
			}
			return result;
		}

		public static TItem Ensure<TItem>( this TItem target ) where TItem : class
		{
			if ( target == null )
			{
				throw new ArgumentException( Resources.Message_Null, "target" );
			}
			return target;
		}
	}
}