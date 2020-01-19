using DragonSpark.Model.Selection;
using System;
using System.Reflection;

namespace DragonSpark.Reflection.Types
{
	public sealed class TypeMetadata : Select<Type, TypeInfo>
	{
		public static TypeMetadata Default { get; } = new TypeMetadata();

		TypeMetadata() : base(x => x.GetTypeInfo()) {}
	}
}