using System;
using System.Linq;
using System.Reflection;
using DragonSpark.Aspects;
using DragonSpark.ComponentModel;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Threading;

namespace DragonSpark.TypeSystem
{
	public interface IAttributeProvider
	{
		Attribute[] GetAttributes( [Required]MemberInfo member, [Required]Type attributeType, bool inherit );
	}

	[Synchronized]
	public class AttributeProvider : IAttributeProvider
	{
		public static AttributeProvider Instance { get; } = new AttributeProvider();

		[Reference]
		readonly IMemberInfoLocator locator;

		AttributeProvider() : this( MemberInfoLocator.Instance )
		{ }

		public AttributeProvider( IMemberInfoLocator locator )
		{
			this.locator = locator;
		}

		[Cache]
		public Attribute[] GetAttributes( MemberInfo member, Type attributeType, bool inherit )
		{
			var located = locator.Locate( member ) ?? member;
			var result = located.IsDefined( attributeType, inherit ) ? located.GetCustomAttributes( attributeType, inherit ).ToArray() : Default<Attribute>.Items;
			return result;
		}
	}

}