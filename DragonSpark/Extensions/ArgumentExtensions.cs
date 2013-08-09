using System;
using System.Diagnostics.Contracts;
using DragonSpark.Properties;

namespace DragonSpark.Extensions
{
	public static class ArgumentExtensions
	{
		[Pure]
		public static string EnsureNotEmpty( this string target )
		{
			Contract.Ensures( !string.IsNullOrEmpty( Contract.Result<string>() ) );
			if ( string.IsNullOrEmpty( target ) )
			{
				throw new ArgumentException( Resources.Message_NullOrEmpty, "target" );
			}
			return target;
		}

		public static TItem EnsureOfType<TItem>( this TItem target, Type type ) where TItem : class
		{
			Contract.Requires( type != null );
			Contract.Ensures( Contract.Result<TItem>() != null );
			Contract.Ensures( type.IsAssignableFrom( Contract.Result<TItem>().GetType() ) );
			var result = target.Ensure<TItem>();
			var resultType = result.GetType();
			if ( !type.IsAssignableFrom( resultType ) )
			{
				throw new ArgumentException( string.Format( "'{0}' does not derrive from type '{1}'.", resultType.AssemblyQualifiedName, type.AssemblyQualifiedName ), "target" );
			}
			return result;
		}

		[Pure]
		public static TItem Ensure<TItem>( this TItem target ) where TItem : class
		{
			Contract.Ensures( Contract.Result<TItem>() != null );
			if ( target == null )
			{
				throw new ArgumentException( Resources.Message_Null, "target" );
			}
			return target;
		}

		/*[Pure]
		public static TItem Assume<TItem>( this TItem target ) where TItem : class
		{
			Contract.Ensures( Contract.Result<TItem>() != null );
			Contract.Ensures( !Contract.Result<TItem>().IsDefault() );
			Contract.Assume( target != null );
			Contract.Assume( !target.IsDefault() );
			return target;
		}

		[Pure]
		public static TItem AssumeValue<TItem>( this TItem target )
		{
			Contract.Ensures( !Contract.Result<TItem>().IsDefault() );
			Contract.Assume( !target.IsDefault() );
			return target;
		}*/
	}
}