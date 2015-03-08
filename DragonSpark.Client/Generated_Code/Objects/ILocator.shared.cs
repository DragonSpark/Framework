using System;
using DragonSpark.Extensions;
using Activator = DragonSpark.Runtime.Activator;

namespace DragonSpark.Objects
{
	public interface ILocator
	{
		object Find( object key );
	}

	public interface IItemProfile
	{
		Type ItemType { get; }
		string ItemName { get; }
	}

	public static class ItemProfileExtensions
	{
		public static TResult Activated<TResult>( this IItemProfile target )
		{
			var result = target.ItemType.Transform( x => Activator.CreateNamedInstance<TResult>( x, target.ItemName ) );
			return result;
		}
	}
}