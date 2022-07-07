using DragonSpark.Model.Selection.Stores;
using System;
using System.Collections.Generic;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class ContentIdentifierStore : StandardTable<Type, HashSet<int>>
{
	public ContentIdentifierStore() : base(_ => new HashSet<int>()) {}
}