using DragonSpark.Activation;
using DragonSpark.Aspects;
using DragonSpark.ComponentModel;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Threading;
using System;
using System.Linq;
using System.Reflection;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.Extensions
{
	public static class AttributeProviderExtensions
	{
		public static bool IsDecoratedWith<TAttribute>( this MemberInfo target, bool inherit = false ) where TAttribute : Attribute
		{
			var result = target.GetAttribute<TAttribute>( inherit ) != null;
			return result;
		}

		public static TAttribute GetAttribute<TAttribute>( this MemberInfo target, bool inherit = false ) where TAttribute : Attribute
		{
			var result = target.GetAttributes<TAttribute>( inherit ).FirstOrDefault();
			return result;
		}

		public static TAttribute[] GetAttributes<TAttribute>( this MemberInfo target, bool inherit = false ) where TAttribute : Attribute
		{
			var provider = Activator.Current.Activate<IAttributeProvider>() ?? AttributeProvider.Instance;
			var result = provider.GetAttributes( target, typeof(TAttribute), inherit ).Cast<TAttribute>().ToArray();
			return result;
		}
	}

	public interface IAttributeProvider
	{
		Attribute[] GetAttributes( [Required]MemberInfo member, [Required]Type attributeType, bool inherit );
	}

	[Synchronized]
	public class AttributeProvider : IAttributeProvider
	{
		public static AttributeProvider Instance { get; } = new AttributeProvider();

		public static Attribute[] Empty { get; } = new Attribute[0];

		[Reference]readonly IMemberInfoLocator locator;

		AttributeProvider() : this( MemberInfoLocator.Instance )
		{}

		public AttributeProvider( IMemberInfoLocator locator )
		{
			this.locator = locator;
		}

		[Cache]
		public Attribute[] GetAttributes( MemberInfo member, Type attributeType, bool inherit )
		{
			var located = locator.Locate( member ) ?? member;
			var result = located.IsDefined( attributeType, inherit ) ? located.GetCustomAttributes( attributeType, inherit ).ToArray() : Empty;
			return result;
		}
	}
}