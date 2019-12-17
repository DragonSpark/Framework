﻿using DragonSpark.Reflection.Types;
using System;
using System.Reflection;

namespace DragonSpark.Model.Selection.Adapters
{
	public class TypeSelector<T> : Selector<T, Type>
	{
		public TypeSelector(ISelect<T, Type> subject) : base(subject) {}

		public MetadataSelector<T> Metadata() => new MetadataSelector<T>(Select(TypeMetadata.Default).Get());
	}

	public class MetadataSelector<T> : Selector<T, TypeInfo>
	{
		public MetadataSelector(ISelect<T, TypeInfo> subject) : base(subject) {}

		public TypeSelector<T> AsType() => new TypeSelector<T>(Select(TypeSelector.Default).Get());
	}
}