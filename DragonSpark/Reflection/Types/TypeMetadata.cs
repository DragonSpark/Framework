using System;
using System.Reflection;
using DragonSpark.Model.Selection;

namespace DragonSpark.Reflection.Types
{
	public sealed class TypeMetadata : Select<Type, TypeInfo>
	{
		public static TypeMetadata Default { get; } = new TypeMetadata();

		TypeMetadata() : base(x => x.GetTypeInfo()) {}
	}
}